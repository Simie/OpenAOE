using OpenAOE.Services;

namespace OpenAOE.Systems
{
    public class MainWindowSystem : ISystem
    {
        private readonly IMainWindow _sdlMainWindow;

        public string Name
        {
            get { return nameof(MainWindowSystem); }
        }

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