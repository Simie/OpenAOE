using System.Collections.Generic;

namespace OpenAOE.Engine.Entity
{
    interface IEntityService
    {
        /// <summary>
        /// List of entities that have been added during the current tick.
        /// </summary>
        IReadOnlyList<IEntity> AddedEntities { get; }

        /// <summary>
        /// List of entities that have been removed during the current tick.
        /// </summary>
        IReadOnlyList<IEntity> RemovedEntities { get; }

        /// <summary>
        /// List of all entities in the simulation.
        /// </summary>
        IReadOnlyList<IEntity> Entities { get; }

        /// <summary>
        /// Create a new entity with the given <paramref name="prototype"/>.
        /// </summary>
        /// <param name="prototype"></param>
        /// <returns></returns>
        IEntity CreateEntity(string prototype);

        /// <summary>
        /// Remove <paramref name="entity"/> from the simulation.
        /// </summary>
        void RemoveEntity(IEntity entity);
    }
}
