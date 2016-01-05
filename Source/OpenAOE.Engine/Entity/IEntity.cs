using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity
{
    interface IEntity
    {
        uint Id { get; }

        bool HasComponent<T>() where T : class, IComponent;
        T Previous<T>() where T : class, IComponent;
        T Current<T>() where T : class, IComponent;
        T Modify<T>() where T : class, IComponent;

        bool WasModified<T>() where T : class, IComponent;
    }
}
