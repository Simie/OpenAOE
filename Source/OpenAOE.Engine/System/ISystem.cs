using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System
{
    public interface ISystem
    {
        string Name { get; }
    }

    public interface IEntitySystem : ISystem
    {
        IComponentFilter Filter { get; }
    }
}
