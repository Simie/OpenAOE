using OpenAOE.Engine.System;
using OpenAOE.Services.Sdl;

namespace OpenAOE.Systems.Rendering.Sdl
{
    [ExecuteOrder(typeof(ClearRenderSystem), ExecuteOrderAttribute.Positions.After)]
    public class TerrainRenderSystem : ISystem
    {
        public string Name => nameof(TerrainRenderSystem);

        private SdlMainWindow _sdlWindow;

        public TerrainRenderSystem(SdlMainWindow sdlWindow)
        {
            _sdlWindow = sdlWindow;
        }

        public void Tick()
        {

        }
    }
}