using Ninject.Modules;
using OpenAOE.Engine;

namespace OpenAOE.Games.AGE2
{
    public class Age2Module : NinjectModule
    {
        public override void Load()
        {
            Bind<IEngineModule>().To<EngineModule>();
        }
    }
}
