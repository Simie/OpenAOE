using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Exceptions;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Entity.Implementation
{
    internal sealed class RuntimeEntity : IEntity
    {
        public uint Id { get; }

        /// <summary>
        /// The key of the EntityTemplate used to construct this entity. Can be null if no template was used.
        /// </summary>
        internal string Prototype { get; set; }

        // TODO: Switch to using a SparseList like Forge does for much faster access to components.
        private readonly IDictionary<ComponentAccessor, ComponentContainer> _components;
        private readonly EntityDirtyTracker _dirtyTracker = new EntityDirtyTracker();
        private readonly IEventDispatcher _eventDispatcher;

        public RuntimeEntity(uint id, IEnumerable<IComponent> components, IEventDispatcher eventDispatcher)
        {
            Id = id;
            _eventDispatcher = eventDispatcher;

            _components =
                components.Select(p => new ComponentContainer(p))
                          .ToDictionary(
                              k => new ComponentAccessor(k.Current.Type),
                              v => v);
        }

        public RuntimeEntity(EntityData data, IEventDispatcher eventDispatcher)
            : this(data.Id, data.Components, eventDispatcher) {}

        public bool HasComponent<T>() where T : class, IComponent
        {
            return _components.ContainsKey(ComponentMap<T>.Accessor);
        }

        public T Current<T>() where T : class, IComponent
        {
            ComponentContainer c;
            if (!_components.TryGetValue(ComponentMap<T>.Accessor, out c))
            {
                throw new ComponentAccessException(Id, typeof(T), ComponentAccessException.Reasons.ComponentDoesNotExist);
            }

            return (T) c.Current;
        }

        public T Modify<T>() where T : class, IWriteableComponent
        {
            var accessor = WriteableComponentMap<T>.Accessor;

            ComponentContainer c;
            if (!_components.TryGetValue(accessor, out c))
            {
                throw new ComponentAccessException(Id, typeof(T), ComponentAccessException.Reasons.ComponentDoesNotExist);
            }

            if (!_dirtyTracker.TrySetDirty(accessor))
            {
                throw new ComponentAccessException(Id, typeof (T),
                    ComponentAccessException.Reasons.ComponentAlreadyAccessed);
            }

            _eventDispatcher.Post(new EntityComponentModified(Id, accessor));
            return (T)_components[accessor].Next;
        }

        /// <summary>
        /// Commit all changes to dirty components. Not thread safe.
        /// </summary>
        public void Commit()
        {
            foreach (var kv in _components)
            {
                if (!_dirtyTracker.TrySetDirty(kv.Key))
                {
                    kv.Value.CommitChanges();
                }
            }

            _dirtyTracker.Reset();
        }
    }
}
