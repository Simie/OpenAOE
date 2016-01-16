using Ninject.Modules;
using OpenAOE.Engine.Implementation;

namespace OpenAOE.Engine
{
    /// <summary>
    /// Defines the bindings used by NInject to construct the engine.
    /// </summary>
    public class EngineModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEngineFactory>().To<EngineFactory>();
        }
    }
}
