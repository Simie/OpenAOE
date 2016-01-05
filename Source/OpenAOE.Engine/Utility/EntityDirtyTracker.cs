using System;
using System.Collections.Generic;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    internal sealed class EntityDirtyTracker
    {
        private readonly HashSet<int> _dirtyComponents = new HashSet<int>();

        public bool IsDirty(ComponentAccessor accessor)
        {
            var id = accessor.Id;
            return _dirtyComponents.Contains(id);
        }

        public void SetDirty(ComponentAccessor accessor)
        {
            var id = accessor.Id;

            if (_dirtyComponents.Contains(id))
                throw new InvalidOperationException($"Component {accessor.ComponentType} is already dirty.");

            _dirtyComponents.Add(id);
        }

        public void Reset()
        {
            _dirtyComponents.Clear();
        }

        public void Reset(ComponentAccessor accessor)
        {
            _dirtyComponents.Remove(accessor.Id);
        }
    }
}
