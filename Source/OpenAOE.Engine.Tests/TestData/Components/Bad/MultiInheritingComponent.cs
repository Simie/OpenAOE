using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Tests.TestData.Components.Bad
{
    public interface IMultiInheritingWritableComponent : IWriteableComponent<ISimpleComponent>,
        IWriteableComponent<IOtherSimpleComponent>
    {
        
    }
}
