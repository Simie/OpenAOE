using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public class TileTransform : Component<TileTransform>
    {
        public Point Position { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }

        public override void CopyTo(TileTransform other)
        {
            other.Position = Position;
            other.Width = Width;
            other.Height = Height;
        }
    }
}
