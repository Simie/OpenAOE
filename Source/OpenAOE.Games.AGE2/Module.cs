using Ninject.Modules;
using OpenAOE.Engine;
using OpenAOE.Engine.System;
using OpenAOE.Games.AGE2.Implementation;
using OpenAOE.Games.AGE2.Systems;

namespace OpenAOE.Games.AGE2
{
    public class Module : NinjectModule, IEngineModule
    {
        public override void Load()
        {
            // Bind Services
            Bind<ITimeService, TimeService>().To<TimeService>().InSingletonScope();

            // Bind Systems
            Bind<ISystem>().To<TimeSystem>();
            Bind<ISystem>().To<UnitMoveSystem>();
        }
    }
}
