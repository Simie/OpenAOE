using System;

namespace OpenAOE.Services
{
    public sealed class MouseEvent
    {
        public readonly int X;
        public readonly int Y;

        public MouseEvent(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public interface IInputService
    {
        event EventHandler<MouseEvent> MouseDown;
    }
}
