using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine.System
{
    public static class Triggers
    {
        /// <summary>
        /// Get notified when an entity is added to the system list.
        /// </summary>
        /// <remarks>
        /// This is called every time the entity is added, not just when the entity is created.
        /// When loading an existing snapshot this will be called for every entity that matches
        /// the system filter.
        /// </remarks>
        public interface IOnEntityAdded
        {
            void OnEntityAdded(EngineEntity entity);
        }

        /// <summary>
        /// Receive an update tick for every entity that matches the system filter.
        /// </summary>
        public interface IOnEntityTick
        {
            void OnTick(EngineEntity entity);
        }

        /// <summary>
        /// Notification when entity is removed from the simulation.
        /// </summary>
        public interface IOnEntityRemoved
        {
            void OnEntityRemoved(EngineEntity entity);
        }

        /// <summary>
        /// Receive a callback for any command of type <typeparamref name="T"/> that
        /// is inputted into the simulation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IOnCommand<T> where T : Command
        {
            void OnCommand(T command);
        }
    }
}
