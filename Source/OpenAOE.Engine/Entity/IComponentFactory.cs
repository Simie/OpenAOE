using System;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity
{
    public interface IComponentFactory
    {
        T Create<T>() where T : IComponent;
        IComponent Create(Type interfaceType);
    }
}
