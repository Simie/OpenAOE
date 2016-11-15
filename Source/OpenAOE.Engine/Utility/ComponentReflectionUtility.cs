using System;
using System.Linq;
using Ninject.Infrastructure.Language;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    internal static class ComponentReflectionUtility
    {
        /// <summary>
        /// Returns the IComponent based interface that describes a component.
        /// </summary>
        /// <param name="concreteComponent">A component class inheriting from <see cref="Component{TThis,TRead,TWrite}" />.</param>
        /// <returns>The interface type.</returns>
        public static Type GetReadOnlyComponentInterface(Type concreteComponent)
        {
            var genericParams = GetComponentBase(concreteComponent).GetGenericArguments();
            return genericParams[1];
        }

        /// <summary>
        /// Returns the IWriteableComponent based interface that describes a component.
        /// </summary>
        /// <param name="concreteComponent">A component class inheriting from <see cref="Component{TThis,TRead,TWrite}" />.</param>
        /// <returns>The interface type.</returns>
        public static Type GetWriteOnlyComponentInterface(Type concreteComponent)
        {
            var genericParams = GetComponentBase(concreteComponent).GetGenericArguments();
            return genericParams[2];
        }

        private static Type GetComponentBase(Type concreteComponent)
        {
            var componentType = concreteComponent.GetAllBaseTypes()
                .SingleOrDefault(
                    p => p.IsGenericType && (p.GetGenericTypeDefinition() == typeof(Component<,,>)));

            if (componentType == null)
                throw new ArgumentException("Type does not inherit from Component<,,>");

            return componentType;
        }
    }
}
