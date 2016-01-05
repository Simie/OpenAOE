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

        public void SetIsDirty<T>() where T : IComponent
        {
            _dirtyTracker.SetDirty<T>();
        }

        public bool WasModified<T>() where T : IComponent
        {
            return _dirtyTracker.IsDirty<T>();
        }
    }
}
