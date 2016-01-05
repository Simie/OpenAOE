using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public interface ITransform : IComponent
    {
        FixVector2 Position { get; }
    }

    public interface IWriteableTransform : IWriteableComponent
    {
        FixVector2 Position { set; }
    }

    class Transform : Component<Transform, ITransform, IWriteableTransform>, ITransform, IWriteableTransform
    {
        public FixVector2 Position { get; set; }

        public override void CopyTo(Transform other)
        {
            other.Position = Position;
        }
    }
}
