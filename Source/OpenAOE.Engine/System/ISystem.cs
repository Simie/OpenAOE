using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.System
{
    public interface ISystem
    {
        IComponentFilter Filter { get; }
    }
}
