using System;
using System.Threading;
using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public interface IHealth : IComponent
    {
        /// <summary>
        /// Value from 0-1 with this entity's current health.
        /// </summary>
        float Value { get; }
    }

    public interface IWriteableHealth : IWriteableComponent<IHealth>
    {
        void AdjustValue(float amount);
    }

    class Health : Component<Health, IHealth, IWriteableHealth>, IHealth, IWriteableHealth, IAsyncComponent
    {
        private float _value;

        public float Value
        {
            get { return _value; }
            private set { _value = value; }
        }

        public void AdjustValue(float amount)
        {
            // TODO: Implement Interlocked.Add call for fixed maths
            throw new NotImplementedException();
        }

        public override void CopyTo(Health other)
        {
            other.Value = Value;
        }
    }
}
