using System.Collections.Generic;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Ninject.Extensions.Logging;
using Ninject.Parameters;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Implementation
{
    // I'm not entirely happy with this method of using IoC, but I can't think of a better way
    // to keep things simple and keep services seperate between instances of the engine,
    // while allowing passing existing entities in etc.
    // Well, it won't be hard to change this later. - SM
    internal class EngineFactory : IEngineFactory
    {
        private readonly IKernel _kernel;
        private readonly ILogger _logger;

        public EngineFactory(IKernel kernel, ILogger logger)
        {
            _kernel = kernel;
            _logger = logger;
        }

        public IEngine Create(IReadOnlyCollection<EntityData> snapshot, IReadOnlyCollection<EntityTemplate> templates)
        {
            _logger.Info("Creating new RuntimeEngine instance");

            // Create a child kernel for this new instance.
            var kernel = new ChildKernel(_kernel, new InternalEngineModule());

            kernel.Bind<IEntityTemplateProvider>().ToConstant(new RuntimeEntityTemplateProvider(templates));
            var eventPoster = kernel.Get<IEventPoster>();

            IList<IEntity> existingEntities = new List<IEntity>();
            foreach (var entity in snapshot)
            {
                existingEntities.Add(new RuntimeEntity(entity.Id, entity.Components, eventPoster));
            }

            kernel.Get<IEntityService>(new ConstructorArgument("entities", existingEntities));

            return kernel.Get<IEngine>();
        }
    }
}
