using System;
using Ninject;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity.Implementation
{
    public sealed class ComponentFactory : IComponentFactory
    {
        private readonly IKernel _kernel;

        public ComponentFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public T Create<T>() where T : IComponent
        {
            return _kernel.Get<T>();
        }

        public IComponent Create(Type interfaceType)
        {
            return (IComponent) _kernel.Get(interfaceType);
        }
    }
}
