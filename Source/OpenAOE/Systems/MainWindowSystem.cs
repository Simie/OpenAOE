using OpenAOE.Services;

namespace OpenAOE.Systems
{
    public class MainWindowSystem : ISystem
    {
        public string Name => nameof(MainWindowSystem);

        private readonly IMainWindow _sdlMainWindow;

        public MainWindowSystem(IMainWindow sdlMainWindow)
        {
            _sdlMainWindow = sdlMainWindow;
        }

        public void Tick()
        {
            _sdlMainWindow.PumpEvents();
        }
    }
}
