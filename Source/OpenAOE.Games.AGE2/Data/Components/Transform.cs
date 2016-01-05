using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public class Transform : Component<Transform>
    {
        public FixVector2 Position { get; set; }

        public override void CopyTo(Transform other)
        {
            other.Position = Position;
        }
    }
}
