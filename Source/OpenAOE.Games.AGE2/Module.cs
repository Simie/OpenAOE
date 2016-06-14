using Ninject.Modules;
using OpenAOE.Engine;

namespace OpenAOE.Games.AGE2
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<IEngineModule>().To<Age2Module>();
        }
    }
}
