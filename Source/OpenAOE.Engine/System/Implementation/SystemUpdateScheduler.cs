using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System.Implementation
{
    /// <summary>
    /// Creates a set of "bursts" (groups of systems that can be executed in parallel) from a
    /// collection of SystemInstances.
    /// </summary>
    /// TODO: Collect systems into "Bursts" that can be executed concurrently.
    /// TODO: Use IAccessGate to prevent access to IEntityService.CreateEntity during concurrent system execution.
    internal class SystemUpdateScheduler
    {
        public class UpdateBurst
        {
            public IReadOnlyCollection<ISystemInstance> Systems { get; }

            public UpdateBurst(IReadOnlyCollection<ISystemInstance> systems)
            {
                Systems = systems;
            }
        }

        public IReadOnlyList<UpdateBurst> UpdateBursts { get; private set; }

        public SystemUpdateScheduler(IReadOnlyList<ISystemInstance> systemInstances)
        {
            var systems = systemInstances.Select(p => p.System).ToList();
            var sorted = ExecuteOrderSorter.Sort(systems);

            var systemList = systemInstances.ToList();
            // TODO
            systemList.Sort((p, q) => sorted.IndexOf(p.System).CompareTo(sorted.IndexOf(q.System)));

            UpdateBursts = new List<UpdateBurst>()
            {
                new UpdateBurst(systemList)
            };
        }
    }
}
