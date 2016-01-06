using Ninject.Extensions.NamedScope;
using Ninject.Modules;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine
{
    /// <summary>
    /// Defines the bindings used by NInject to construct the engine.
    /// </summary>
    public class EngineModule : NinjectModule
    {
        public const string SimulationScope = "EngineScope";

        public override void Load()
        {
            // TODO: Use Context Preservation to enable factories to use the correctly scoped services if constructed post-SimulationInstance request
            Bind<IComponentFactory>().To<ComponentFactory>().InNamedScope(SimulationScope);

            /*Bind<ISimulationInstance>().To<SimulationInstance>().DefinesNamedScope(SimulationScope);

            Bind<IEntityService>().To<EntityServiceImpl>().InNamedScope(SimulationScope);
            Bind<ISpacialLookupService>().To<SpacialLookupImplementation>().InNamedScope(SimulationScope);

            Bind<ISystem>().To<TestUpdatePlayerSystem>();
            Bind<ISystem>().To<TestUpdateSystem>();*/
        }
    }
}
