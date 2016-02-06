using Ninject;
using Ninject.Modules;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.System;
using OpenAOE.Engine.System.Implementation;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Implementation
{
    class InternalEngineModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEngine>().To<RuntimeEngine>().InSingletonScope();

            Bind<EventQueue>().ToSelf().InSingletonScope();
            Bind<IEventDispatcher>().ToMethod(c => c.Kernel.Get<EventQueue>());

            Bind<IEntityService, RuntimeEntityService>().To<RuntimeEntityService>().InSingletonScope();
            Bind<ISystemManager>().To<RuntimeSystemManager>().InSingletonScope();
            Bind<IComponentFactory>().To<ComponentFactory>().InSingletonScope();
        }
    }
}
