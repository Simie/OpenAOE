using Ninject.Extensions.Logging;
using OpenAOE.Engine;
using OpenAOE.Services;

namespace OpenAOE.Systems
{
    public class GameEngineTickSystem : ISystem
    {
        private readonly IGameEngineService _gameEngineService;

        public GameEngineTickSystem(ILogger logger, IGameEngineService gameEngineService)
        {
            _gameEngineService = gameEngineService;
        }

        public string Name
        {
            get { return nameof(GameEngineTickSystem); }
        }

        public void Tick()
        {
            if (_gameEngineService.Engine == null)
                return;

            EngineTickInput input;
            //if (i == 1) {
            //    input = new EngineTickInput(new Command[] { new MoveCommand(0, new FixVector2(20, 20)) });
            //} else {
            input = new EngineTickInput();
            //}

            var tick = _gameEngineService.Engine.Tick(input);
            tick.Start();
            tick.Wait();
            _gameEngineService.Engine.Synchronize();
        }
    }
}
