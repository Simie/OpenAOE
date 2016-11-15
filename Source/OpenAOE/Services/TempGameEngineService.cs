using System;
using System.Collections.Generic;
using Ninject.Extensions.Logging;
using OpenAOE.Engine;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;
using OpenAOE.Games.AGE2.Data.Commands;
using OpenAOE.Games.AGE2.Data.Components;

namespace OpenAOE.Services
{
    public class TempGameEngineService : IGameEngineService
    {
        private static readonly List<EntityTemplate> TestTemplates = new List<EntityTemplate>
        {
            new EntityTemplate("Unit", new List<IComponent>
            {
                new Transform(),
                new Movable
                {
                    TargetPosition = null,
                    MoveSpeed = 5f
                }
            })
        };

        private static readonly List<EntityData> TestData = new List<EntityData>
        {
            new EntityData(0, new List<IComponent>
            {
                new Transform(),
                new Movable
                {
                    TargetPosition = new FixVector2(20, 20),
                    MoveSpeed = 8f
                }
            })
        };

        public IEngine Engine { get; }

        private readonly List<Command> _commands = new List<Command>();
        private readonly IEngineFactory _engineFactory;
        private readonly IInputService _inputService;

        private readonly ILogger _logger;

        public TempGameEngineService(ILogger logger, IEngineFactory engineFactory, IInputService inputService)
        {
            _logger = logger;
            _engineFactory = engineFactory;
            _inputService = inputService;

            _inputService.MouseDown += OnMouseDown;

            _logger.Info("Creating a game engine");
            Engine = engineFactory.Create(TestData, TestTemplates);
        }

        public void Tick()
        {
            if (Engine == null)
                return;

            if (_commands.Count > 0)
                _logger.Info("Executing tick with {0} commands.", _commands.Count);

            var input = new EngineTickInput(_commands);

            _commands.Clear();

            var tick = Engine.Tick(input);
            tick.Start();
            tick.Wait();
            Engine.Synchronize();

            foreach (var resultEvent in tick.Result.Events)
                EngineEvent?.Invoke(this, new GameEngineEventArgs(resultEvent, Engine));
        }

        public event EventHandler<EngineChangedEventArgs> EngineChanged;

        public event EventHandler<GameEngineEventArgs> EngineEvent;

        private void OnMouseDown(object sender, MouseEvent mouseEvent)
        {
            _commands.Add(new MoveCommand(0, new FixVector2(mouseEvent.X, mouseEvent.Y)));
        }
    }
}
