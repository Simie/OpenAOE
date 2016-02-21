using System.Collections.Generic;

namespace OpenAOE.Engine.System
{
    /// <summary>
    /// The System Manager maintains a list of system instances and their target entities.
    /// </summary>
    internal interface ISystemManager
    {
        /// <summary>
        /// A list of all systems running in the service.
        /// </summary>
        IReadOnlyList<ISystemInstance> Systems { get; }

        /// <summary>
        /// Supply a list of entities to be added to systems that pass their filter.
        /// </summary>
        /// <param name="entityList"></param>
        void AddEntities(IReadOnlyList<Entity.EngineEntity> entityList);

        /// <summary>
        /// Supply a list of entities to removed from systems that pass their filter.
        /// </summary>
        /// <param name="entityList"></param>
        void RemoveEntities(IReadOnlyList<Entity.EngineEntity> entityList);
    }
}
