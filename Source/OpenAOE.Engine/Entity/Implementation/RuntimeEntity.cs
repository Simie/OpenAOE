using System;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity.Implementation
{
    class RuntimeEntity : IEntity
    {
        public uint Id { get; }

        public RuntimeEntity(uint id)
        {
            Id = id;
        }

        public bool HasComponent<T>() where T : class, IComponent
        {
            throw new NotImplementedException();
        }

        public T Current<T>() where T : class, IComponent
        {
            throw new NotImplementedException();
        }

        public TWrite Modify<TWrite, TRead>() where TWrite : class, IWriteableComponent<TRead> where TRead : IComponent
        {
            throw new NotImplementedException();
        }

        public T Modify<T>() where T : class, IWriteableComponent
        {
            throw new NotImplementedException();
        }
    }
}
