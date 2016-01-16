using System.Collections.Generic;
using Ninject;
using Ninject.Extensions.ChildKernel;
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

        public EngineFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IEngine Create(IReadOnlyCollection<EntityData> entities)
        {
            // Create a child kernel for this new instance.
            var kernel = new ChildKernel(_kernel, new InternalEngineModule());
            var eventPoster = kernel.Get<IEventPoster>();

            IList<IEntity> existingEntities = new List<IEntity>();
            foreach (var entity in entities)
            {
                existingEntities.Add(new RuntimeEntity(entity.Id, entity.Components, eventPoster));
            }

            kernel.Get<IEntityService>(new ConstructorArgument("entities", existingEntities));

            return kernel.Get<IEngine>();
        }
    }
}
