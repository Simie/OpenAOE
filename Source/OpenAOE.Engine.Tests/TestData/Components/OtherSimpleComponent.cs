using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Tests.TestData.Components
{
    public interface IOtherSimpleComponent : IComponent
    {
        int Value { get; }
    }

    public interface IWriteableOtherSimpleComponent : IWriteableComponent<IOtherSimpleComponent>
    {
        int Value { set; }
    }

    class OtherSimpleComponent : Component<OtherSimpleComponent, IOtherSimpleComponent, IWriteableComponent>, IOtherSimpleComponent, IWriteableOtherSimpleComponent
    {
        public int Value { get; set; }

        public override void CopyTo(OtherSimpleComponent other)
        {
            other.Value = Value;
        }
    }
}
