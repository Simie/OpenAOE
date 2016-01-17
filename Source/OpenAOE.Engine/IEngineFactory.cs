using System.Collections.Generic;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine
{
    public class EntityData
    {
        public readonly uint Id;
        public readonly IReadOnlyCollection<IComponent> Components;

        public EntityData(uint id, IReadOnlyCollection<IComponent> components)
        {
            Id = id;
            Components = components;
        }
    }

    /// <summary>
    /// Public methods for creating an engine from an existing data set.
    /// </summary>
    public interface IEngineFactory
    {
        /// <summary>
        /// Create a new engine with the data.
        /// </summary>
        /// <param name="snapshot">Collection of entities to include in the simulation.</param>
        /// <param name="templates">Collection of entities to use as templates during the simulation.</param>
        /// <returns>A new instance of <see cref="IEngine"/> with the snapshot loaded.</returns>
        IEngine Create(IReadOnlyCollection<EntityData> snapshot, IReadOnlyCollection<EntityTemplate> templates);
    }
}
