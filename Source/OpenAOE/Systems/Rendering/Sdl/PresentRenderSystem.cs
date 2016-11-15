using OpenAOE.Engine.System;
using OpenAOE.Services.Sdl;
using SDL2;

namespace OpenAOE.Systems.Rendering.Sdl
{
    [ExecuteOrder(typeof(ClearRenderSystem), ExecuteOrderAttribute.Positions.After)]
    public class PresentRenderSystem : ISystem
    {
        public string Name => nameof(PresentRenderSystem);

        private readonly SdlMainWindow _sdlWindow;

        public PresentRenderSystem(SdlMainWindow sdlWindow)
        {
            _sdlWindow = sdlWindow;
        }

        public void Tick()
        {
            SDL.SDL_RenderPresent(_sdlWindow.RenderHandle);
        }
    }
}
