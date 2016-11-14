using System;
using JetBrains.Annotations;
using OpenAOE.Engine;
using OpenAOE.Engine.Data;

namespace OpenAOE.Services
{
    public class EngineChangedEventArgs : EventArgs
    {
        public IEngine NewEngine { get; }

        public EngineChangedEventArgs(IEngine newEngine)
        {
            NewEngine = newEngine;
        }
    }

    public class GameEngineEventArgs : EventArgs
    {
        public GameEngineEventArgs(Event @event, IEngine engine)
        {
            Event = @event;
            Engine = engine;
        }

        public Event Event { get; }
        public IEngine Engine { get; }
    }

    public interface IGameEngineService
    {
        event EventHandler<EngineChangedEventArgs> EngineChanged;
        event EventHandler<GameEngineEventArgs> EngineEvent;
         
        [CanBeNull]
        IEngine Engine { get; }

        void Tick();
    }
}