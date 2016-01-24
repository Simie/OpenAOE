using OpenAOE.Engine.Data;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System
{
    abstract class FilteredSystem : ISystem
    {
        public abstract IComponentFilter Filter { get; }
    }

    internal abstract class FilteredSystem<T> : FilteredSystem 
        where T : class, IComponent
    {
        public override IComponentFilter Filter { get; } = new GenericComponentFilter<T>();
    }

    internal abstract class FilteredSystem<T1, T2> : FilteredSystem 
        where T1 : class, IComponent
        where T2 : class, IComponent
    {
        public override IComponentFilter Filter { get; } = new GenericComponentFilter<T1, T2>();
    }

    internal abstract class FilteredSystem<T1, T2, T3> : FilteredSystem 
        where T1 : class, IComponent
        where T2 : class, IComponent
        where T3 : class, IComponent
    {
        public override IComponentFilter Filter { get; } = new GenericComponentFilter<T1, T2, T3>();
    }
}
