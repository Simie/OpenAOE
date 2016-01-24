using System;
using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine.System.Implementation
{
    internal class RuntimeSystemManager : ISystemManager
    {
        public IReadOnlyList<ISystemInstance> Systems => _systems;

        private readonly List<RuntimeSystemInstance> _systems;

        public RuntimeSystemManager(IEnumerable<ISystem> systems)
        {
            _systems = systems.Select(p => new RuntimeSystemInstance(p)).ToList();
        }

        public void AddEntities(IReadOnlyList<IEntity> entityList)
        {
            foreach (var system in _systems)
            {
                foreach (var entity in entityList)
                {
                    if (system.Filter.Filter(entity))
                    {
                        system.Entities.Add(entity);
                        // TODO: Callback
                    }
                }
            }
        }

        public void RemoveEntities(IReadOnlyList<IEntity> entityList)
        {
            foreach (var system in _systems)
            {
                foreach (var entity in entityList)
                {
                    if (system.Entities.Remove(entity))
                    {
                        // TODO: Callback
                    }
                }
            }
        }
    }
}
