using System;
using System.ComponentModel;

namespace OpenAOE.Engine.System
{
    // TODO: Replace with Filter(IEntity) method to make use of HasComponent<T>
    abstract class FilteredSystem : ISystem
    {
        public abstract Type[] ComponentFilter { get; }
    }

    internal abstract class FilteredSystem<T> : FilteredSystem where T : IComponent
    {
        public override Type[] ComponentFilter { get; } = {typeof (T)};
    }

    internal abstract class FilteredSystem<T1, T2> : FilteredSystem where T1 : IComponent where T2 : IComponent
    {
        public override Type[] ComponentFilter { get; } = { typeof(T1), typeof(T2) };
    }

    internal abstract class FilteredSystem<T1, T2, T3> : FilteredSystem where T1 : IComponent where T2 : IComponent where T3 : IComponent
    {
        public override Type[] ComponentFilter { get; } = {typeof (T1), typeof (T2), typeof (T3)};
    }
}
