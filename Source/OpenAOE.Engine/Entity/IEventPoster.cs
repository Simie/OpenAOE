using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity
{
    /// <summary>
    /// Interface for events to be posted to be consumed at some point in the future.
    /// This should be thread safe.
    /// </summary>
    public interface IEventPoster
    {
        void Post<T>(T e) where T : Event;
    }
}
