using Ninject;
using Ninject.Modules;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Implementation
{
    class InternalEngineModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEngine>().To<RuntimeEngine>().InSingletonScope();

            Bind<EventQueue>().ToSelf().InSingletonScope();
            Bind<IEventPoster>().ToMethod(c => c.Kernel.Get<EventQueue>());

            Bind<IEntityService>().To<RuntimeEntityService>().InSingletonScope();
            Bind<IComponentFactory>().To<ComponentFactory>().InSingletonScope();
        }
    }
}
