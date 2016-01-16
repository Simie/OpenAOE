using System;
using System.Collections.Generic;
using OpenAOE.Engine.Data;

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
        /// <returns></returns>
        IEngine Create(IReadOnlyCollection<EntityData> entities);
    }
}
