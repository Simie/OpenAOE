using System;
using Ninject.Extensions.Logging;
using OpenAOE.Services.Config;
using SDL2;

namespace OpenAOE.Services.Sdl
{
    public class SdlMainWindow : IDisposable, IMainWindow
    {
        public int Width
        {
            get
            {
                int w, h;
                SDL.SDL_GetWindowSize(_window, out w, out h);
                return w;
            }
        }

        public int Height
        {
            get
            {
                int w, h;
                SDL.SDL_GetWindowSize(_window, out w, out h);
                return h;
            }
        }

        public IntPtr RenderHandle
        {
            get { return _renderer; }
        }

        private readonly ILogger _logger;
        public event EventHandler CloseRequested;

        private IntPtr _window;
        private IntPtr _renderer;
        private readonly IWriteableConfig<int> _windowWidth;
        private readonly IWriteableConfig<int> _windowHeight;

        private bool _refreshWindow;

        public SdlMainWindow(ILogger logger, IConfigService configService)
        {
            _logger = logger;

            _windowWidth = configService.GetWritableConfig("Window", "Width", 1024);
            _windowHeight = configService.GetWritableConfig("Window", "Height", 768);

            _windowWidth.Changed += OnWindowSizeConfigChanged;
            _windowHeight.Changed += OnWindowSizeConfigChanged;

            _logger.Info("Creating window and renderer with size {0}x{1}", _windowWidth.Value, _windowHeight.Value);

            if (SDL.SDL_CreateWindowAndRenderer(_windowWidth.Value, _windowHeight.Value, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE, out _window,
                out _renderer) < 0)
            {
                var error = SDL.SDL_GetError();
                _logger.Fatal("Failed to create window. SDL Error: {0}", error);
                throw new Exception($"Failed to create SDL window. {error}");
            }

            SDL.SDL_SetWindowTitle(_window, "OpenAOE");
        }

        private void OnWindowSizeConfigChanged(IWriteableConfig<int> sender, ConfigChangedEvent<int> args)
        {
            _refreshWindow = true;
        }

        public void PumpEvents()
        {
            SDL.SDL_Event evnt;

            while (SDL.SDL_PollEvent(out evnt) != 0)
            {
                switch (evnt.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                    {
                        _logger.Info("SDL_Quit");
                        CloseRequested?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                    case SDL.SDL_EventType.SDL_WINDOWEVENT:
                    {
                        if (evnt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED)
                        {
                            int h, w;
                            SDL.SDL_GetWindowSize(_window, out w, out h);
                            _windowWidth.Value = w;
                            _windowHeight.Value = h;
                        }
                        break;
                    }
                }
            }

            if (_refreshWindow)
            {
                var width = _windowWidth.Value;
                var height = _windowHeight.Value;
                _logger.Info("Resizing window to {0}x{1}", width, height);
                SDL.SDL_SetWindowSize(_window, width, height);
                SDL.SDL_RenderSetLogicalSize(_renderer, width, height);
                _refreshWindow = false;
            }
        }

        public void Dispose()
        {
            SDL.SDL_DestroyRenderer(_window);
            SDL.SDL_DestroyWindow(_window);
        }
    }
}
