using System;
using System.Collections.Generic;
using Ninject.Extensions.Logging;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System
{
    internal class SystemInstance
    {
        public ISystem System;

        public bool HasOnEntityAdded;
        public bool HasOnEntityRemoved;
        public bool HasGlobalTick;
        public bool HasEntityTick;
        public Type[] Filter;

        public SystemInstance(ISystem system)
        {
            System = system;

            HasOnEntityAdded = System is IOnEntityAdded;
            HasOnEntityRemoved = System is IOnEntityRemoved;
            HasGlobalTick = System is IOnGlobalTick;
            HasEntityTick = System is IOnEntityTick;
        }
    }

    internal class SystemController : IOnEntityTick, IOnEntityAdded, IOnGlobalTick, IOnEntityRemoved
    {
        internal IList<SystemInstance> Systems { get { return _systems.AsReadOnly(); } }

        private readonly ILogger _log;
        private readonly IEntityService _entityService;
        private readonly List<SystemInstance> _systems = new List<SystemInstance>();

        public SystemController(ILogger log, IEntityService entityService, ISystem[] systems)
        {
            _log = log;
            _entityService = entityService;

            var sortedSystems = ExecuteOrderSorter.Sort(systems);

            foreach (var system in sortedSystems)
            {
                AddSystem(system);
            }
        }

        void AddSystem(ISystem sys)
        {
            var instance = new SystemInstance(sys);
            _systems.Add(instance);
        }

        public void OnTick(IEntity entity)
        {
            
        }

        public void OnEntityAdded(IEntity entity)
        {
            
        }

        public void OnGlobalTick(IEntity globalEntity)
        {
            foreach (var sys in _systems)
            {
                if (sys.HasGlobalTick)
                {
                    ((IOnGlobalTick) sys.System).OnGlobalTick(globalEntity);
                }
            }
        }

        public void OnEntityRemoved(IEntity entity)
        {
            foreach (var sys in _systems)
            {
                if (sys.HasOnEntityRemoved)
                {
                    ((IOnEntityRemoved) sys.System).OnEntityRemoved(entity);
                }
            }
        }
    }
}
