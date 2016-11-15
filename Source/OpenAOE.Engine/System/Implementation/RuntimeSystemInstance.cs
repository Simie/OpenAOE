using System;
using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System.Implementation
{
    internal class RuntimeSystemInstance : ISystemInstance
    {
        public List<EngineEntity> Entities { get; } = new List<EngineEntity>();

        public bool IsEntitySystem { get; }

        public IComponentFilter Filter
        {
            get
            {
                if (!IsEntitySystem) throw new InvalidOperationException();
                return ((IEntitySystem) System).Filter;
            }
        }

        IReadOnlyList<EngineEntity> ISystemInstance.Entities => Entities;

        public IReadOnlyList<ICommandHandler> CommandHandlers { get; }

        public ISystem System { get; }

        public bool HasEntityTick { get; }

        public bool HasEntityAdd { get; }

        public bool HasEntityRemove { get; }

        public bool HasTick { get; }

        public RuntimeSystemInstance(ISystem system)
        {
            System = system;
            IsEntitySystem = System is IEntitySystem;

            HasEntityAdd = System is Triggers.IOnEntityAdded;
            HasEntityRemove = System is Triggers.IOnEntityRemoved;
            HasEntityTick = System is Triggers.IOnEntityTick;
            HasTick = System is Triggers.IOnTick;

            CommandHandlers = CommandHandlerUtil.GetCommandHandlers(system).ToList();
        }
    }
}
