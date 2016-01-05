using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine.System
{
    /// <summary>
    /// Get notified when an entity is added to the simulation.
    /// </summary>
    internal interface IOnEntityAdded
    {
        void OnEntityAdded(IEntity entity);
    }
    
    /// <summary>
    /// Receive every tick
    /// </summary>
    internal interface IOnGlobalTick
    {
        void OnGlobalTick(IEntity globalEntity);
    }

    /// <summary>
    /// Receive an update tick for every entity (unless system is a <see cref="FilteredSystem"/>)
    /// </summary>
    internal interface IOnEntityTick
    {
        void OnTick(IEntity entity);
    }

    /// <summary>
    /// Notification when entity is removed from the simulation.
    /// </summary>
    internal interface IOnEntityRemoved
    {
        void OnEntityRemoved(IEntity entity);
    }
}
