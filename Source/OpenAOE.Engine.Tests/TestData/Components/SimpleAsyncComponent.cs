using System.Threading;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Tests.TestData.Components
{
    public interface ISimpleAsyncComponent : IComponent
    {
        int Value { get; }
    }

    public interface IWriteableSimpleAsyncComponent : IWriteableComponent<ISimpleComponent>, IAsyncComponent
    {
        void AddValue(int amount);
    }

    internal class SimpleAsyncComponent :
        Component<SimpleAsyncComponent, ISimpleAsyncComponent, IWriteableSimpleAsyncComponent>,
        ISimpleAsyncComponent, IWriteableSimpleAsyncComponent
    {
        public int Value => _value;

        private int _value;

        public void AddValue(int amount)
        {
            Interlocked.Add(ref _value, amount);
        }

        public override void CopyTo(SimpleAsyncComponent other)
        {
            other._value = Value;
        }
    }
}
