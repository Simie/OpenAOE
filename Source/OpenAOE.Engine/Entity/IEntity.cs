using JetBrains.Annotations;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Exceptions;

namespace OpenAOE.Engine.Entity
{
    public interface IEntity
    {
        /// <summary>
        /// Unique ID used to refer to this entity.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Check if entity has a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A component interface inheriting from <see cref="IComponent"/>.</typeparam>
        /// <returns>True if the entity has a component of type <typeparamref name="T"/>.</returns>
        bool HasComponent<T>() where T : class, IComponent;

        /*/// <summary>
        /// Check if component of type <typeparamref name="T"/> has already been modified during this tick.
        /// WARNING: Do not base any logic off this method, as it is not deterministic (system order of execution may be different on another machine).
        /// </summary>
        /// <typeparam name="T">A component interface inheriting from <see cref="IComponent"/>.</typeparam>
        /// <returns>True if the component of type <typeparamref name="T"/> was modified during this tick.</returns>
        // TODO: Consider removing this. Is it required to be part of the IEntity interface?
        bool WasModified<T>() where T : class, IComponent;*/

        /// <summary>
        /// Returns the component of type <typeparamref name="T"/> from the previous tick.
        /// </summary>
        /// <exception cref="ComponentAccessException">Thrown if the component does not exist on the entity.</exception>
        /// <typeparam name="T">The interface type of the component you wish to fetch.</typeparam>
        /// <returns>The component of type <typeparamref name="T"/> from the previous tick.</returns>
        [CanBeNull]
        T Previous<T>() where T : class, IComponent;

        /// <summary>
        /// Returns the component of type <typeparamref name="T"/> from the current tick.
        /// </summary>
        /// <exception cref="ComponentAccessException">Thrown if the component does not exist on the entity.</exception>
        /// <typeparam name="T">The interface type of the component you wish to fetch.</typeparam>
        /// <returns>The component of type <typeparamref name="T"/> from the current tick.</returns>
        T Current<T>() where T : class, IComponent;

        /// <summary>
        /// Returns the writeable component of type <typeparamref name="TWrite"/> for the next tick.
        /// </summary>
        /// <exception cref="ComponentAccessException">Thrown if the component does not exist on the entity or has already been accessed during this tick.</exception>
        /// <typeparam name="TWrite">The interface type of the component you wish to write too.</typeparam>
        /// <returns>The writeable component of type <typeparamref name="TWrite"/>.</returns>
        TWrite Modify<TWrite>() where TWrite : class, IWriteableComponent;
    }
}
