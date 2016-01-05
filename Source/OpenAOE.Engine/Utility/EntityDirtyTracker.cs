using System;
using System.Collections.Generic;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    internal sealed class EntityDirtyTracker
    {
        private readonly HashSet<int> _dirtyComponents = new HashSet<int>();

        /// <summary>
        /// Mark the component of type <paramref name="T"/> as dirty.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the component of type <paramref name="T"/> has already been flagged as dirty.</exception>
        /// <typeparam name="T">Component</typeparam>
        public void SetDirty<T>() where T : IComponent
        {
            var id = ComponentMap<T>.Accessor.Id;

            if(_dirtyComponents.Contains(id))
                throw new InvalidOperationException($"Component {typeof(T)} is already dirty.");

            _dirtyComponents.Add(id);
        }

        public bool IsDirty<T>() where T : IComponent
        {
            var id = ComponentMap<T>.Accessor.Id;
            return _dirtyComponents.Contains(id);
        }
    }
}
