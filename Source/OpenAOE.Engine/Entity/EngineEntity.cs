using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Exceptions;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Entity
{
    public sealed class EngineEntity : IReadOnlyEntity
    {
        /// <summary>
        /// The key of the EntityTemplate used to construct this entity. Can be null if no template was used.
        /// </summary>
        internal string Prototype { get; set; }

        /// <summary>
        /// Unique ID used to refer to this entity.
        /// </summary>
        public uint Id { get; }

        // TODO: Switch to using a SparseList like Forge does for much faster access to components.
        private readonly IDictionary<ComponentAccessor, ComponentContainer> _components;
        private readonly EntityDirtyTracker _dirtyTracker = new EntityDirtyTracker();
        private readonly IEventDispatcher _eventDispatcher;

        internal EngineEntity(uint id, IEnumerable<IComponent> components, IEventDispatcher eventDispatcher)
        {
            Id = id;
            _eventDispatcher = eventDispatcher;

            _components =
                components.Select(p => new ComponentContainer(p))
                    .ToDictionary(
                        k => new ComponentAccessor(k.Current.Type),
                        v => v);
        }

        /// <summary>
        /// Check if entity has a component of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">A component interface inheriting from <see cref="IComponent" />.</typeparam>
        /// <returns>True if the entity has a component of type <typeparamref name="T" />.</returns>
        public bool HasComponent<T>() where T : class, IComponent
        {
            return _components.ContainsKey(ComponentMap<T>.Accessor);
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="T" /> from the current tick.
        /// </summary>
        /// <exception cref="ComponentAccessException">Thrown if the component does not exist on the entity.</exception>
        /// <typeparam name="T">The interface type of the component you wish to fetch.</typeparam>
        /// <returns>The component of type <typeparamref name="T" /> from the current tick.</returns>
        public T Current<T>() where T : class, IComponent
        {
            ComponentContainer c;
            if (!_components.TryGetValue(ComponentMap<T>.Accessor, out c))
                throw new ComponentAccessException(Id, typeof(T), ComponentAccessException.Reasons.ComponentDoesNotExist);

            return (T) c.Current;
        }

        /// <summary>
        /// Returns the writeable component of type <typeparamref name="TWrite" /> for the next tick.
        /// </summary>
        /// <exception cref="ComponentAccessException">
        /// Thrown if the component does not exist on the entity or has already been
        /// accessed during this tick.
        /// </exception>
        /// <typeparam name="TWrite">The interface type of the component you wish to write too.</typeparam>
        /// <returns>The writeable component of type <typeparamref name="TWrite" />.</returns>
        public TWrite Modify<TWrite>() where TWrite : class, IWriteableComponent
        {
            var accessor = WriteableComponentMap<TWrite>.Accessor;

            ComponentContainer c;
            if (!_components.TryGetValue(accessor, out c))
                throw new ComponentAccessException(Id, typeof(TWrite),
                    ComponentAccessException.Reasons.ComponentDoesNotExist);

            if (!_dirtyTracker.TrySetDirty(accessor))
                throw new ComponentAccessException(Id, typeof(TWrite),
                    ComponentAccessException.Reasons.ComponentAlreadyAccessed);

            _eventDispatcher.Post(new EntityComponentModified(Id, accessor));
            return (TWrite) _components[accessor].Next;
        }

        /// <summary>
        /// Commit all changes to dirty components. Not thread safe.
        /// </summary>
        internal void Commit()
        {
            foreach (var kv in _components)
                if (!_dirtyTracker.TrySetDirty(kv.Key))
                    kv.Value.CommitChanges();

            _dirtyTracker.Reset();
        }
    }
}
