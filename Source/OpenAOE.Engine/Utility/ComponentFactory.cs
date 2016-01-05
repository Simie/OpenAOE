using System;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Utility
{
    public static class ComponentFactory
    {
        public static T Create<T>() where T : IComponent
        {
            throw new NotImplementedException();
        }

        public static IComponent Create(Type interfaceType)
        {
            throw new NotImplementedException();
        }
    }
}
