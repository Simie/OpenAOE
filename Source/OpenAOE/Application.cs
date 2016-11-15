using System;
using Ninject.Extensions.Logging;
using OpenAOE.Services;

namespace OpenAOE
{
    internal class Application : IDisposable
    {
        private readonly ILogger _logger;
        private readonly IMainWindow _mainWindow;
        private readonly SystemManager _systemManager;
        private bool _shouldRun = true;

        public Application(ILogger logger, IMainWindow mainWindow, SystemManager systemManager)
        {
            _logger = logger;
            _mainWindow = mainWindow;
            _systemManager = systemManager;
            mainWindow.CloseRequested += MainWindowOnCloseRequested;

            _logger.Info("Starting Application");
        }

        public void Dispose()
        {
        }

        private void MainWindowOnCloseRequested(object sender, EventArgs eventArgs)
        {
            _logger.Info("MainWindow close requested.");
            _shouldRun = false;
        }

        public void Run()
        {
            _logger.Info("Running");

            while (_shouldRun)
                _systemManager.Tick();

            _logger.Info("Finished");
        }
    }
}
