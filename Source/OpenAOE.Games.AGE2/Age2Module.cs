using Ninject.Modules;
using OpenAOE.Engine;
using OpenAOE.Engine.System;
using OpenAOE.Games.AGE2.Implementation;
using OpenAOE.Games.AGE2.Services;
using OpenAOE.Games.AGE2.Services.Implementation;
using OpenAOE.Games.AGE2.Systems;

namespace OpenAOE.Games.AGE2
{
    class Age2Module : NinjectModule, IEngineModule
    {
        public override void Load()
        {
            // Bind Services
            Bind<ITimeService, TimeService>().To<TimeService>().InSingletonScope();
            Bind<IPlayerService, PlayerService>().To<PlayerService>().InSingletonScope();

            // Bind Systems
            Bind<ISystem>().To<PlayerSystem>();
            Bind<ISystem>().To<TimeSystem>();
            Bind<ISystem>().To<UnitMoveSystem>();
        }
    }
}