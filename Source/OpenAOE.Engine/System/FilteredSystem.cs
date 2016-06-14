using OpenAOE.Engine.Data;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System
{
    public abstract class FilteredSystem : IEntitySystem
    {
        public virtual string Name => GetType().Name;
        public abstract IComponentFilter Filter { get; }
    }

    public abstract class FilteredSystem<T> : FilteredSystem 
        where T : class, IComponent
    {
        public override IComponentFilter Filter { get; } = ComponentFilter.Matches<T>();
    }

    public abstract class FilteredSystem<T1, T2> : FilteredSystem 
        where T1 : class, IComponent
        where T2 : class, IComponent
    {
        public override IComponentFilter Filter { get; } = ComponentFilter.Matches<T1, T2>();
    }

    public abstract class FilteredSystem<T1, T2, T3> : FilteredSystem 
        where T1 : class, IComponent
        where T2 : class, IComponent
        where T3 : class, IComponent
    {
        public override IComponentFilter Filter { get; } = ComponentFilter.Matches<T1, T2, T3>();
    }
}
