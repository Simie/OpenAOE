using System.Collections.Generic;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System.Implementation
{
    internal class RuntimeSystemInstance : ISystemInstance
    {
        IReadOnlyList<Entity.EngineEntity> ISystemInstance.Entities => Entities;

        public ISystem System { get; }

        public IComponentFilter Filter { get { return System.Filter; } }

        public List<Entity.EngineEntity> Entities { get; } = new List<Entity.EngineEntity>();

        public RuntimeSystemInstance(ISystem system)
        {
            System = system;
        }
    }
}
