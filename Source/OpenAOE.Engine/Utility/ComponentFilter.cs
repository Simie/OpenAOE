using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Engine.Utility
{
    public static class ComponentFilter
    {
        /// <summary>
        /// A filter that matches any components.
        /// </summary>
        public static readonly IComponentFilter Any = new AnyComponentFilter();

        /// <summary>
        /// A filter that matches entities with a <typeparamref name="T" /> component.
        /// </summary>
        public static IComponentFilter Matches<T>() where T : class, IComponent
        {
            return new GenericComponentFilter<T>();
        }

        /// <summary>
        /// A filter that matches entities with both a <typeparamref name="T1" /> and a <typeparamref name="T2" /> component.
        /// </summary>
        public static IComponentFilter Matches<T1, T2>()
            where T1 : class, IComponent
            where T2 : class, IComponent
        {
            return new GenericComponentFilter<T1, T2>();
        }

        /// <summary>
        /// A filter that matches entities with a <typeparamref name="T1" />, <typeparamref name="T2" />
        /// and <typeparamref name="T3" /> component.
        /// </summary>
        public static IComponentFilter Matches<T1, T2, T3>()
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
        {
            return new GenericComponentFilter<T1, T2, T3>();
        }

        /// <summary>
        /// A filter that matches entities with a <typeparamref name="T1" />,
        /// <typeparamref name="T2" />, <typeparamref name="T3" /> and <typeparamref name="T4" /> component.
        /// </summary>
        public static IComponentFilter Matches<T1, T2, T3, T4>()
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
            where T4 : class, IComponent
        {
            return new GenericComponentFilter<T1, T2, T3, T4>();
        }

        private sealed class AnyComponentFilter : IComponentFilter
        {
            public bool Filter(IHasComponents target)
            {
                return true;
            }
        }
    }
}
