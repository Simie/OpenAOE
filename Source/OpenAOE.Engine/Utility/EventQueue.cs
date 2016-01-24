using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    // TODO
    class EventQueue : IEventDispatcher
    {
        private readonly ConcurrentQueue<Event> _eventQueue = new ConcurrentQueue<Event>(); 

        public void Post<T>(T e) where T : Event
        {
            _eventQueue.Enqueue(e);
        }

        public IEnumerable<Event> DequeueAll()
        {
            ICollection<Event> events = new Collection<Event>();
            Event e;

            while (_eventQueue.TryDequeue(out e))
            {
                events.Add(e);
            }

            return events;
        } 
    }
}
