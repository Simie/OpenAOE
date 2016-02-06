using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.System;
using OpenAOE.Engine.System.Implementation;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Implementation
{
    internal sealed class RuntimeEngine : IEngine
    {
        public IReadOnlyCollection<IEntity> Entities
        {
            get { return EntityService.Entities; }
        }

        enum States
        {
            Idle,
            Updating,
            AwaitingSync
        }

        internal RuntimeEntityService EntityService;
        internal ISystemManager SystemManager;

        internal EventQueue EventQueue;

        internal SystemUpdateScheduler SystemUpdateScheduler;

        private States _state = States.Idle;

        public RuntimeEngine(RuntimeEntityService entityService, ISystemManager systemManager, EventQueue eventQueue)
        {
            if (entityService == null)
            {
                throw new ArgumentNullException(nameof(entityService));
            }

            if (systemManager == null)
            {
                throw new ArgumentNullException(nameof(systemManager));
            }

            EventQueue = eventQueue;
            EntityService = (RuntimeEntityService)entityService;
            SystemManager = systemManager;
            SystemUpdateScheduler = new SystemUpdateScheduler(SystemManager.Systems);

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

                EntityService.CommitRemoved();

                if (EntityService.AddedEntities.Count > 0)
                {
                    SystemManager.AddEntities(EntityService.RemovedEntities);
                }

                EntityService.CommitAdded();

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

            EntityService.CommitAdded();
            _state = States.Idle;
        }
    }
}
