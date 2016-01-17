using System.Collections.Concurrent;

namespace OpenAOE.Engine.Utility
{
    /// <summary>
    /// Tracks whether a component is dirty or not. This class is thread-safe.
    /// </summary>
    internal sealed class EntityDirtyTracker
    {
        private readonly ConcurrentDictionary<int, bool> _dirtyComponents = new ConcurrentDictionary<int, bool>();

        /// <summary>
        /// Set the component with <paramref name="accessor"/> to be dirty.
        /// </summary>
        /// <param name="accessor">Component accessor to mark as dirty.</param>
        /// <returns>True if successfully marked component as dirty. False if component is already dirty.</returns>
        public bool TrySetDirty(ComponentAccessor accessor)
        {
            var id = accessor.Id;

            if (_dirtyComponents.TryAdd(id, true))
                return true;

            if (!_dirtyComponents.TryUpdate(id, true, false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reset all components to "not dirty".
        /// </summary>
        public void Reset()
        {
            foreach (var kv in _dirtyComponents)
            {
                _dirtyComponents.TryUpdate(kv.Key, false, kv.Value);
            }
        }

        /// <summary>
        /// Reset component with <paramref name="accessor"/> to be not-dirty.
        /// </summary>
        /// <param name="accessor">Component accessor to mark as not-dirty.</param>
        public void Reset(ComponentAccessor accessor)
        {
            _dirtyComponents.AddOrUpdate(accessor.Id, false, (i, b) => false);
        }
    }
}
