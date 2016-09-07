using Ninject.Modules;
using OpenAOE.Services;
using Ninject.Extensions.Conventions;
using OpenAOE.Systems;

namespace OpenAOE
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<SystemManager>().ToSelf().InSingletonScope();
            Bind<Application>().ToSelf().InSingletonScope();
            Bind<IMainWindow, SdlMainWindow>().To<SdlMainWindow>().InSingletonScope();

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