using OpenAOE.Engine.System;
using OpenAOE.Services.Sdl;

namespace OpenAOE.Systems.Rendering.Sdl
{
    [ExecuteOrder(typeof(ClearRenderSystem), ExecuteOrderAttribute.Positions.After)]
    public class PresentRenderSystem : ISystem
    {
        private readonly SdlMainWindow _sdlWindow;
        public string Name => nameof(PresentRenderSystem);

        public PresentRenderSystem(SdlMainWindow sdlWindow)
        {
            _sdlWindow = sdlWindow;
        }

        public void Tick()
        {
            SDL2.SDL.SDL_RenderPresent(_sdlWindow.RenderHandle);
        }
    }
}