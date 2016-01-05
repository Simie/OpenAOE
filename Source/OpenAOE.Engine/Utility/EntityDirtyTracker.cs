using System;
using System.Collections.Generic;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    sealed class EntityDirtyTracker
    {
        private readonly HashSet<int> _dirtyComponents = new HashSet<int>();

        public void SetDirty<T>() where T : IComponent
        {
            var id = DataMap<T>.Accessor.Id;

            if(_dirtyComponents.Contains(id))
                throw new InvalidOperationException($"Component {typeof(T)} is already dirty.");

            _dirtyComponents.Add(id);
        }

        public bool IsDirty<T>() where T : IComponent
        {
            var id = DataMap<T>.Accessor.Id;
            return _dirtyComponents.Contains(id);
        }
    }
}
