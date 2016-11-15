using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine.Utility
{
    public class GenericComponentFilter<T> : IComponentFilter where T : class, IComponent
    {
        public bool Filter(IHasComponents target)
        {
            return target.HasComponent<T>();
        }
    }

    public class GenericComponentFilter<T1, T2> : IComponentFilter
        where T1 : class, IComponent
        where T2 : class, IComponent
    {
        public bool Filter(IHasComponents target)
        {
            return target.HasComponent<T1>() && target.HasComponent<T2>();
        }
    }

    public class GenericComponentFilter<T1, T2, T3> : IComponentFilter
        where T1 : class, IComponent
        where T2 : class, IComponent
        where T3 : class, IComponent
    {
        public bool Filter(IHasComponents target)
        {
            return target.HasComponent<T1>() && target.HasComponent<T2>() && target.HasComponent<T3>();
        }
    }

    public class GenericComponentFilter<T1, T2, T3, T4> : IComponentFilter
        where T1 : class, IComponent
        where T2 : class, IComponent
        where T3 : class, IComponent
        where T4 : class, IComponent
    {
        public bool Filter(IHasComponents target)
        {
            return target.HasComponent<T1>() && target.HasComponent<T2>() && target.HasComponent<T3>() &&
                   target.HasComponent<T4>();
        }
    }
}
