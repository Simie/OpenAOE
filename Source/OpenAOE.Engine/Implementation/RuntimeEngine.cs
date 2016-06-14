using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ninject.Extensions.Logging;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.System;
using OpenAOE.Engine.System.Implementation;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Implementation
{
    internal sealed class RuntimeEngine : IEngine
    {
        public IReadOnlyCollection<IReadOnlyEntity> Entities
        {
            get { return EntityService.Entities; }
        }

        enum States
        {
            Idle,
            Updating,
            AwaitingSync
        }

        internal readonly RuntimeEntityService EntityService;
        internal readonly ISystemManager SystemManager;

        internal readonly EventQueue EventQueue;
        private readonly ILogger _logger;

        internal SystemUpdateScheduler SystemUpdateScheduler;

        internal AccessGate AddEntityAccessGate;

        private States _state = States.Idle;

        public RuntimeEngine(RuntimeEntityService entityService, ISystemManager systemManager, EventQueue eventQueue, ILogger logger)
        {
            AddEntityAccessGate = new AccessGate();
            EventQueue = eventQueue;
            _logger = logger;
            EntityService = entityService;
            EntityService.AddEntityAccessGate = AddEntityAccessGate;
            SystemManager = systemManager;
            SystemUpdateScheduler = new SystemUpdateScheduler(SystemManager.Systems);

            foreach (var updateBurst in SystemUpdateScheduler.UpdateBursts)
            {
                _logger.Info($"Update Burst: {string.Join(", ", updateBurst.Systems.Select(p => p.System.Name))}");
            }

            // Add any entities that are already loaded into the engine.
            SystemManager.AddEntities(entityService.Entities);
        }

        public Task<EngineTickResult> Tick(EngineTickInput input)
        {
            if (_state != States.Idle)
            {
                throw new InvalidOperationException($"RuntimeEngine cannot Tick() while while in state `{_state}`");
            }

            _state = States.Updating;

            var tick = new Task<EngineTickResult>(() =>
            {
                _state = States.AwaitingSync;

                // TODO: Handle commands input

                // Clear the "RemovedEntities" list from last frame.
                EntityService.CommitRemoved();

                // Update all systems
                foreach (var updateBurst in SystemUpdateScheduler.UpdateBursts)
                {
                    AddEntityAccessGate.IsLocked = updateBurst.Systems.Count > 1;

                    Parallel.ForEach(updateBurst.Systems, (system) =>
                    {
                        if (system.HasTick)
                        {
                            ((Triggers.IOnTick)system.System).OnTick();
                        }

                        if (system.HasEntityTick)
                        {
                            Parallel.ForEach(system.Entities, (entity) =>
                            {
                                ((Triggers.IOnEntityTick) system.System).OnTick(entity);
                            });
                        }
                    });
                }

                // Add any entities from this frame to the system manager
                if (EntityService.AddedEntities.Count > 0)
                {
                    SystemManager.AddEntities(EntityService.AddedEntities);
                }

                // Process the removal from systems of any entities that were removed in this frame.
                if (EntityService.RemovedEntities.Count > 0)
                {
                    SystemManager.RemoveEntities(EntityService.RemovedEntities);
                }

                // Gather all the events that were added during this update step
                var events = EventQueue.DequeueAll();

                return new EngineTickResult(events);
            });

            return tick;
        }

        public void Synchronize()
        {
            if (_state != States.AwaitingSync)
            {
                throw new InvalidOperationException($"RuntimeEngine cannot Synchronize() while while in state `{_state}`");
            }
            
            // Add all the "Added" entities to the main entity list.
            EntityService.CommitAdded();
            _state = States.Idle;
        }
    }
}
