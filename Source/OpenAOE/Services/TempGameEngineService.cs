using System;
using System.Collections.Generic;
using Ninject.Extensions.Logging;
using OpenAOE.Engine;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;
using OpenAOE.Games.AGE2.Data.Components;

namespace OpenAOE.Services
{
    public class TempGameEngineService : IGameEngineService
    {
        public event EventHandler<EngineChangedEventArgs> EngineChanged;
        public event EventHandler<GameEngineEventArgs> EngineEvent;

        public IEngine Engine { get { return _engine; } }

        private static List<EntityTemplate> TestTemplates = new List<EntityTemplate>()
        {
            new EntityTemplate("Unit", new List<IComponent>()
            {
                new Transform() {},
                new Movable()
                {
                    TargetPosition = null,
                    MoveSpeed = 5f
                }
            })
        };

        private static List<EntityData> TestData = new List<EntityData>()
        {
            new EntityData(0, new List<IComponent>()
            {
                new Transform() {},
                new Movable()
                {
                    TargetPosition = new FixVector2(20, 20),
                    MoveSpeed = 8f
                }
            })
        };

        private readonly ILogger _logger;
        private readonly IEngineFactory _engineFactory;
        private readonly IEngine _engine;

        public TempGameEngineService(ILogger logger, IEngineFactory engineFactory)
        {
            _logger = logger;
            _engineFactory = engineFactory;

            _logger.Info("Creating a game engine");
            _engine = engineFactory.Create(TestData, TestTemplates);
        }

        public void Tick()
        {
            if (Engine == null)
                return;

            EngineTickInput input;
            //if (i == 1) {
            //    input = new EngineTickInput(new Command[] { new MoveCommand(0, new FixVector2(20, 20)) });
            //} else {
            input = new EngineTickInput();
            //}

            var tick = Engine.Tick(input);
            tick.Start();
            tick.Wait();
            Engine.Synchronize();

            foreach (var resultEvent in tick.Result.Events)
            {
                EngineEvent?.Invoke(this, new GameEngineEventArgs(resultEvent, Engine));
            }
        }
    }
}