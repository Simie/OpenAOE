using System;
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

    internal class Health : Component<Health, IHealth, IWriteableHealth>, IHealth, IWriteableHealth, IAsyncComponent
    {
        public float Value { get; private set; }

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
