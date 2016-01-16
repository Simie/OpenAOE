using System.Threading.Tasks;

namespace OpenAOE.Engine
{
    public struct EngineTickInput
    {
        
    }

    public struct EngineTickResult
    {
        
    }

    public interface IEngine
    {
        Task<EngineTickResult> Tick(EngineTickInput input);
    }

    internal class EngineInstance
    {
        
    }
}
