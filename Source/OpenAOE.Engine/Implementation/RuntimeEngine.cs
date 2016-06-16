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

        internal readonly SystemUpdateScheduler SystemUpdateScheduler;

        internal readonly AccessGate AddEntityAccessGate;

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
                // Clear the "RemovedEntities" list from last frame.
                EntityService.CommitRemoved();
                
                // TODO: This is probably not deterministic. Update order is not guaranteeed for multithreaded bursts
                // Also this many loops is nasty.
               foreach (var updateBurst in SystemUpdateScheduler.UpdateBursts)
               {
                   foreach (var sys in updateBurst.Systems)
                   {
                       foreach (var commandHandler in sys.CommandHandlers)
                       {
                            foreach (var command in input.Commands)
                            {
                                if (commandHandler.CanHandle(command))
                                {
                                    commandHandler.OnCommand(command);
                                }
                            }
                        }
                   }
               }
                
                // Update all systems
                foreach (var updateBurst in SystemUpdateScheduler.UpdateBursts)
                {
                    // Lock the entity access gate if this is a multi-threaded burst.
                    AddEntityAccessGate.IsLocked = updateBurst.Systems.Count > 1;
                    // TODO: This still won't be enough to guarantee entity ID consistency.
                    // across different machines, since entity tick is also multithreaded.

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

                _state = States.AwaitingSync;
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

            // Commit all changes to entities.
            // TODO: We want to have the possibility of modifying a component multiple times during a frame
            EntityService.CommitDirty();

            _state = States.Idle;
        }
    }
}
