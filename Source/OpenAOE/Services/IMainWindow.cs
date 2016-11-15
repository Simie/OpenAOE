using System;

namespace OpenAOE.Services
{
    public interface IMainWindow
    {
        int Width { get; }

        int Height { get; }

        void PumpEvents();

        event EventHandler CloseRequested;
    }
}
