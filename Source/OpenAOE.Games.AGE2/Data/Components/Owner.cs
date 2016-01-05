using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public interface IOwner : IComponent
    {
        uint PlayerId { get; }
    }

    public interface IWriteableOwner : IOwner
    {
        new uint PlayerId { get; set; }
    }

    class Owner : Component<Owner>, IWriteableOwner
    {
        public uint PlayerId { get; set; }

        public override void CopyTo(Owner other)
        {
            other.PlayerId = PlayerId;
        }
    }
}
