using System;
using System.Collections.Generic;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System.Implementation
{
    internal class RuntimeSystemInstance : ISystemInstance
    {
        IReadOnlyList<IEntity> ISystemInstance.Entities => Entities;

        public ISystem System { get; }

        public IComponentFilter Filter { get { return System.Filter; } }

        public List<IEntity> Entities { get; } = new List<IEntity>();

        public RuntimeSystemInstance(ISystem system)
        {
            System = system;
        }
    }
}
