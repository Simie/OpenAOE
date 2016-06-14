using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public interface IMovable : IComponent
    {
        FixVector2? TargetPosition { get; }
        float MoveSpeed { get; }
    }

    public interface IWriteableMovable : IWriteableComponent
    {
        FixVector2? TargetPosition { set; }
        float MoveSpeed { set; }
    }

    public class Movable : Component<Movable, IMovable, IWriteableMovable>, IMovable, IWriteableMovable
    {
        public FixVector2? TargetPosition { get; set; }
        public float MoveSpeed { get; set; }

        public override void CopyTo(Movable other)
        {
            other.TargetPosition = TargetPosition;
            other.MoveSpeed = MoveSpeed;
        }
    }
}