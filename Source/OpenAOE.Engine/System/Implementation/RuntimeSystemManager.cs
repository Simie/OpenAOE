using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.Logging;

namespace OpenAOE.Engine.System.Implementation
{
    internal class RuntimeSystemManager : ISystemManager
    {
        private readonly ILogger _logger;
        public IReadOnlyList<ISystemInstance> Systems => _systems;

        private readonly List<RuntimeSystemInstance> _systems;

        public RuntimeSystemManager(IEnumerable<ISystem> systems, ILogger logger)
        {
            _logger = logger;
            _systems = systems.Select(p => new RuntimeSystemInstance(p)).ToList();

            _logger.Info("Initialised with systems: {0}", string.Join(", ", _systems.Select(p => p.System.Name)));
        }

        public void AddEntities(IReadOnlyList<Entity.EngineEntity> entityList)
        {
            foreach (var system in _systems)
            {
                if (!system.IsEntitySystem)
                    continue;

                foreach (var entity in entityList)
                {
                    if (system.Filter.Filter(entity))
                    {
                        system.Entities.Add(entity);

                        if (system.HasEntityAdd)
                        {
                            ((Triggers.IOnEntityAdded)system.System).OnEntityAdded(entity);
                        }
                    }
                }
            }
        }

        public void RemoveEntities(IReadOnlyList<Entity.EngineEntity> entityList)
        {
            foreach (var system in _systems)
            {
                if (!system.IsEntitySystem)
                    continue;

                foreach (var entity in entityList)
                {
                    if (system.Entities.Remove(entity))
                    {
                        if (system.HasEntityRemove)
                        {
                            ((Triggers.IOnEntityRemoved)system.System).OnEntityRemoved(entity);
                        }
                    }
                }
            }
        }
    }
}
