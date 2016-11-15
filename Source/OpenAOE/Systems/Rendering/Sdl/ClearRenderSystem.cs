using Ninject.Extensions.Logging;
using OpenAOE.Engine.System;
using OpenAOE.Services.Sdl;
using SDL2;

namespace OpenAOE.Systems.Rendering.Sdl
{
    [ExecuteOrder(typeof(GameEngineTickSystem), ExecuteOrderAttribute.Positions.After)]
    public class ClearRenderSystem : ISystem
    {
        public string Name => nameof(ClearRenderSystem);

        private readonly ILogger _logger;
        private readonly SdlMainWindow _sdlWindow;

        public ClearRenderSystem(SdlMainWindow sdlWindow, ILogger logger)
        {
            _sdlWindow = sdlWindow;
            _logger = logger;
        }

        public void Tick()
        {
            if (SDL.SDL_SetRenderDrawColor(_sdlWindow.RenderHandle, 32, 30, 35, 255) != 0)
                _logger.Error(SDL.SDL_GetError());
            if (SDL.SDL_RenderClear(_sdlWindow.RenderHandle) != 0)
                _logger.Error(SDL.SDL_GetError());
        }
    }
}
