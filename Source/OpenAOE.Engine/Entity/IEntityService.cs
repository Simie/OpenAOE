using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace OpenAOE.Engine.Entity
{
    public interface IEntityService
    {
        /// <summary>
        /// List of all entities in the simulation.
        /// </summary>
        IReadOnlyList<IEntity> Entities { get; }

        /// <summary>
        /// Get the entity with the specified ID. 
        /// Thread Safe.
        /// </summary>
        /// <param name="id">Unique ID for the entity to be fetched.</param>
        /// <returns>The entity with <paramref name="id"/> as the Id, or null if not found.</returns>
        [CanBeNull]
        IEntity GetEntity(uint id);

        /// <summary>
        /// Create a new entity with the given <paramref name="prototype"/>. 
        /// Can only be called from a synchronous system (i.e. no other systems running at the same time).
        /// </summary>
        /// <remarks>
        /// The reason for the synchronous system restriction is to prevent different IDs being assigned on client machines that execute
        /// in a different order due to thread scheduling.
        /// </remarks>
        /// <param name="prototype"></param>
        /// <exception cref="ArgumentException">If a prototype with name <paramref name="prototype"/> is not found.</exception>
        /// <returns>The created entity.</returns>
        IEntity CreateEntity([NotNull] string prototype);

        /// <summary>
        /// Remove <paramref name="entity"/> from the simulation. 
        /// Thread safe.
        /// </summary>
        void RemoveEntity([NotNull] IEntity entity);
    }
}
