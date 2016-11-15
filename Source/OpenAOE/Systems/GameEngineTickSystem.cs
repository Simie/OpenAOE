using OpenAOE.Services;

namespace OpenAOE.Systems
{
    public class GameEngineTickSystem : ISystem
    {
        public string Name => nameof(GameEngineTickSystem);

        private readonly IGameEngineService _gameEngineService;

        public GameEngineTickSystem(IGameEngineService gameEngineService)
        {
            _gameEngineService = gameEngineService;
        }

        public void Tick()
        {
            _gameEngineService.Tick();
        }
    }
}
