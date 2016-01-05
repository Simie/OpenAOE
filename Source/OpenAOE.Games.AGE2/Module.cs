using Ninject.Modules;

namespace OpenAOE.Games.AGE2
{
    /// <summary>
    /// Defines the bindings used by NInject to construct the engine.
    /// </summary>
    public class Module : NinjectModule
    {
        public const string SimulationScope = "EngineScope";

        public override void Load()
        {
            // TODO: Use Context Preservation to enable factories to use the correctly scoped services if constructed post-SimulationInstance request

            /*Bind<ISimulationInstance>().To<SimulationInstance>().DefinesNamedScope(SimulationScope);

            Bind<IEntityService>().To<EntityServiceImpl>().InNamedScope(SimulationScope);
            Bind<ISpacialLookupService>().To<SpacialLookupImplementation>().InNamedScope(SimulationScope);

            Bind<ISystem>().To<TestUpdatePlayerSystem>();
            Bind<ISystem>().To<TestUpdateSystem>();*/
        }
    }
}
