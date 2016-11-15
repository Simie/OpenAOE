using OpenAOE.Engine.System;
using OpenAOE.Games.AGE2.Data.Components;
using OpenAOE.Services;
using OpenAOE.Services.Sdl;
using OpenAOE.Utilities;
using static SDL2.SDL;

namespace OpenAOE.Systems.Rendering.Sdl
{
    [ExecuteOrder(typeof(TerrainRenderSystem), ExecuteOrderAttribute.Positions.After)]
    [ExecuteOrder(typeof(PresentRenderSystem), ExecuteOrderAttribute.Positions.Before)]
    public class UnitRenderSystem : ISystem
    {
        public string Name => nameof(UnitRenderSystem);

        private readonly SdlMainWindow _sdlWindow;
        private readonly EntityBag _unitBag;

        public UnitRenderSystem(SdlMainWindow sdlWindow, IGameEngineService gameEngineService)
        {
            _sdlWindow = sdlWindow;
            _unitBag = EntityBag.Create<ITransform>(gameEngineService);
        }

        public void Tick()
        {
            SDL_SetRenderDrawColor(_sdlWindow.RenderHandle, 255, 255, 255, 255);

            var rect = new SDL_Rect();

            foreach (var entity in _unitBag.Contents)
            {
                var transform = entity.Current<ITransform>();
                //var unit = entity.Current<IUnit>();

                rect.x = (int) transform.Position.X - 5;
                rect.y = (int) transform.Position.Y - 5;
                rect.w = 10;
                rect.h = 10;

                SDL_RenderDrawRect(_sdlWindow.RenderHandle, ref rect);
            }
        }
    }
}
