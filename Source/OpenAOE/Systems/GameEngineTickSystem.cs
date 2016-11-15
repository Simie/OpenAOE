using Ninject.Extensions.Logging;
using OpenAOE.Engine;
using OpenAOE.Services;

namespace OpenAOE.Systems
{
    public class GameEngineTickSystem : ISystem
    {
        private readonly IGameEngineService _gameEngineService;

        public GameEngineTickSystem(IGameEngineService gameEngineService)
        {
            _gameEngineService = gameEngineService;
        }

        public string Name => nameof(GameEngineTickSystem);

        public void Tick()
        {
            _gameEngineService.Tick();
        }
    }
}
