using System;
using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System.Implementation
{
    internal class RuntimeSystemInstance : ISystemInstance
    {
        public List<Entity.EngineEntity> Entities { get; } = new List<Entity.EngineEntity>();
        IReadOnlyList<Entity.EngineEntity> ISystemInstance.Entities => Entities;

        public IReadOnlyList<ICommandHandler> CommandHandlers { get; }

        public ISystem System { get; }

        public bool IsEntitySystem { get; }

        public bool HasEntityTick { get; }
        public bool HasEntityAdd { get; }
        public bool HasEntityRemove { get; }
        public bool HasTick { get; }

        public IComponentFilter Filter
        {
            get
            {
                if(!IsEntitySystem) throw new InvalidOperationException();
                return ((IEntitySystem) System).Filter;
            }
        }

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
