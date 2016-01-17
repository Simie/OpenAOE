using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
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
        internal EventQueue EventQueue;

        private States _state = States.Idle;

        public RuntimeEngine(IEntityService entityService, EventQueue eventQueue)
        {
            if (entityService == null)
            {
                throw new ArgumentNullException(nameof(entityService));
            }

            if (!(entityService is RuntimeEntityService))
            {
                throw new ArgumentException("Expected entityService to be of type RuntimeEntityService (sorry)", nameof(entityService));
            }

            EventQueue = eventQueue;
            EntityService = (RuntimeEntityService)entityService;
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
