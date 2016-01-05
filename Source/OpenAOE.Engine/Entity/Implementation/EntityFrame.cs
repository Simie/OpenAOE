using OpenAOE.Engine.Data;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Entity.Implementation
{
    /// <summary>
    /// A "frame" contains the state of an entity for a tick.
    /// </summary>
    internal class EntityFrame
    {
        private readonly EntityDirtyTracker _dirtyTracker = new EntityDirtyTracker();

        public void SetIsDirty(ComponentAccessor accessor)
        {
            _dirtyTracker.SetDirty(accessor);
        }

        public bool WasModified<T>(ComponentAccessor accessor)
        {
            return _dirtyTracker.IsDirty(accessor);
        }
    }
}
