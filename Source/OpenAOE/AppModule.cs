using Ninject.Modules;
using OpenAOE.Services;
using Ninject.Extensions.Conventions;
using OpenAOE.Services.Sdl;
using OpenAOE.Systems;

namespace OpenAOE
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<SystemManager>().ToSelf().InSingletonScope();
            Bind<Application>().ToSelf().InSingletonScope();
            Bind<IMainWindow, IInputService, SdlMainWindow>().To<SdlMainWindow>().InSingletonScope();
            Bind<IGameEngineService>().To<TempGameEngineService>().InSingletonScope();

            Kernel.Bind(p =>
            {
                p.FromThisAssembly()
                    .SelectAllClasses()
                    .InheritedFrom<ISystem>()
                    .BindSingleInterface();
            });
        }
    }
}