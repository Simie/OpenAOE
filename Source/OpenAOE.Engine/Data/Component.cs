namespace OpenAOE.Engine.Data
{
    public interface IComponent
    {
        IComponent Clone();
    }
    public abstract class Component<T> : IComponent where T : Component<T>, new()
    { 
        public abstract void CopyTo(T other);

        public T Clone()
        {
            var t = new T();
            CopyTo(t);
            return t;
        }

        IComponent IComponent.Clone()
        {
            return Clone();
        }
    }
}
