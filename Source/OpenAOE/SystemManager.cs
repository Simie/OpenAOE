using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.Logging;
using OpenAOE.Engine.Utility;
using ISystem = OpenAOE.Systems.ISystem;

namespace OpenAOE.Services
{
    public class SystemManager
    {
        private readonly ILogger _logger;
        private readonly IList<ISystem> _systems;

        public SystemManager(IEnumerable<ISystem> systems, ILogger logger)
        {
            _logger = logger;
            _systems = ExecuteOrderSorter.Sort(systems.ToList());

            if (_systems.Count > 0)
            {
                _logger.Info("Initialized with systems: {0}",
                    _systems.Select(p => p.Name).Aggregate((q, s) => q + "," + s));
            }
            else
            {
                _logger.Warn("No systems provided.");
            }
        }

        public void Tick()
        {
            foreach (var system in _systems)
            {
                system.Tick();
            }
        }
    }
}