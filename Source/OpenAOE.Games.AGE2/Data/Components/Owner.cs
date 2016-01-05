using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public interface IOwner : IComponent
    {
        uint PlayerId { get; }
    }

    public interface IWriteableOwner : IWriteableComponent
    {
        uint PlayerId { set; }
    }

    class Owner : Component<Owner, IOwner, IWriteableOwner>, IOwner, IWriteableOwner
    {
        public uint PlayerId { get; set; }

        public override void CopyTo(Owner other)
        {
            other.PlayerId = PlayerId;
        }
    }
}
