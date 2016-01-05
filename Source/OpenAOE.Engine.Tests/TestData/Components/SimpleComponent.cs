using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Tests.TestData.Components
{
    public interface ISimpleComponent : IComponent
    {
        int Value { get; }
    }

    public interface IWriteableSimpleComponent : IWriteableComponent<ISimpleComponent>
    {
        int Value { set; }
    }

    class SimpleComponent : Component<SimpleComponent, ISimpleComponent, IWriteableComponent>, ISimpleComponent, IWriteableSimpleComponent
    {
        public int Value { get; set; }

        public override void CopyTo(SimpleComponent other)
        {
            other.Value = Value;
        }
    }
}
