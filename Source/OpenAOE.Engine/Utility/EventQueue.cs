using System.Collections.Concurrent;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    // TODO
    class EventQueue: IEventPoster
    {
        private readonly ConcurrentQueue<Event> _eventQueue = new ConcurrentQueue<Event>(); 

        public void Post<T>(T e) where T : Event
        {
            //_eventQueue.Enqueue(e);
        }
    }
}
