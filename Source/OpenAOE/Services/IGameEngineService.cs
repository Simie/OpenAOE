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
        public Event Event { get; }

        public IEngine Engine { get; }

        public GameEngineEventArgs(Event @event, IEngine engine)
        {
            Event = @event;
            Engine = engine;
        }
    }

    public interface IGameEngineService
    {
        [CanBeNull]
        IEngine Engine { get; }

        event EventHandler<EngineChangedEventArgs> EngineChanged;

        event EventHandler<GameEngineEventArgs> EngineEvent;

        void Tick();
    }
}
