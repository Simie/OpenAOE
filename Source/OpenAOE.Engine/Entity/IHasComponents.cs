using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity
{
    /// <summary>
    /// Interface for an object that has components and can have the presence of components
    /// queried.
    /// </summary>
    public interface IHasComponents
    {
        /// <summary>
        /// Check if entity has a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A component interface inheriting from <see cref="IComponent"/>.</typeparam>
        /// <returns>True if the entity has a component of type <typeparamref name="T"/>.</returns>
        bool HasComponent<T>() where T : class, IComponent;
    }
}
