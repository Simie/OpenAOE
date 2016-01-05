using System;

namespace OpenAOE.Engine.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public readonly Type ComponentType;

        public ComponentNotFoundException(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}
