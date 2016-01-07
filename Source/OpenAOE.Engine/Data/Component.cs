using System;

namespace OpenAOE.Engine.Data
{
    /// <summary>
    /// Interface to support type-erasure for components.
    /// </summary>
    public interface IComponent
    {
        IComponent Clone();
        void CopyTo(IComponent component);
    }

    /// <summary>
    /// Interface for a writeable component (for type-erasure).
    /// </summary>
    public interface IWriteableComponent
    {
        
    }

    public interface IWriteableComponent<T> : IWriteableComponent where T : IComponent
    {
        
    }

    /// <summary>
    /// Indicates that a component supports async writes (ie can have <code>IEntity.Modify</code> called more than once on it).
    /// </summary>
    public interface IAsyncComponent
    {
        
    }

    /// <summary>
    /// Base class for the concrete implementation of a component. Declares a strong-typed CopyTo method that needs to be implemented
    /// by components.
    /// </summary>
    /// <typeparam name="TThis">The type of the class that is implementing this abstract component.</typeparam>
    /// <typeparam name="TRead">Read-only interface of the component being implemented.</typeparam>
    /// <typeparam name="TWrite">Write-only interface of the component being implemented.</typeparam>
    public abstract class Component<TThis, TRead, TWrite> : IComponent
        where TThis : Component<TThis, TRead, TWrite>, TRead, TWrite, new()
        where TRead : IComponent
        where TWrite : IWriteableComponent
    {
        public abstract void CopyTo(TThis other);

        public TThis Clone()
        {
            var t = new TThis();
            CopyTo(t);
            return t;
        }

        public void CopyTo(IComponent component)
        {
            if(!(component is TThis))
                throw new ArgumentException("component must be of same type to copy", nameof(component));

            var c = ((TThis) component);
            CopyTo(c);
        }

        IComponent IComponent.Clone()
        {
            return Clone();
        }
    }
}
