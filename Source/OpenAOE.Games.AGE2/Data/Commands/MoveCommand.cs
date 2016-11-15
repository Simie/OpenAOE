using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Commands
{
    public sealed class MoveCommand : Command
    {
        public uint Target { get; private set; }

        public FixVector2 Position { get; private set; }

        public MoveCommand(uint target, FixVector2 position)
        {
            Target = target;
            Position = position;
        }
    }
}
