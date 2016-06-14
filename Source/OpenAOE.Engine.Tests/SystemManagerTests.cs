
using System.Linq;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;
using OpenAOE.Engine.System.Implementation;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class SystemManagerTests
    {
        public class TwoComponentSystem : IEntitySystem
        {
            public string Name => nameof(TwoComponentSystem);
            public IComponentFilter Filter => new GenericComponentFilter<ISimpleComponent, IOtherSimpleComponent>();
        }

        public class OneComponentSystem : IEntitySystem
        {
            public string Name => nameof(OneComponentSystem);
            public IComponentFilter Filter => new GenericComponentFilter<ISimpleComponent>();
        }
        
        [Test]
        public void CreatesInstanceForEverySystemInConstructor()
        {
            var systems = new ISystem[]{new TwoComponentSystem(), new OneComponentSystem()};
            var manager = new RuntimeSystemManager(systems, Mock.Of<ILogger>());

            manager.Systems.Count.ShouldBe(2);
            manager.Systems.ShouldAllBe(p => systems.Contains(p.System));
        }

        [Test]
        public void CorrectlyFilterEntitiesIntoSystemInstances()
        {
            var oneComponentSystem = new OneComponentSystem();
            var twoComponentSystem = new TwoComponentSystem();

            var manager = new RuntimeSystemManager(new ISystem[] {oneComponentSystem, twoComponentSystem}, Mock.Of<ILogger>());

            var oneComponentEntity = new EngineEntity(0, new IComponent[] {new SimpleComponent()},
                Mock.Of<IEventDispatcher>());

            var twoComponentEntity = new EngineEntity(0,
                new IComponent[] {new SimpleComponent(), new OtherSimpleComponent(),},
                Mock.Of<IEventDispatcher>());

            manager.AddEntities(new[] {oneComponentEntity, twoComponentEntity});

            manager.Systems.Single(p => p.System == oneComponentSystem).Entities.ShouldContain(oneComponentEntity);
            manager.Systems.Single(p => p.System == oneComponentSystem).Entities.ShouldContain(twoComponentEntity);

            manager.Systems.Single(p => p.System == twoComponentSystem).Entities.ShouldContain(twoComponentEntity);
            manager.Systems.Single(p => p.System == twoComponentSystem).Entities.ShouldNotContain(oneComponentEntity);
        }


        [Test]
        public void RemovesEntityFromSystem()
        {
            var oneComponentSystem = new OneComponentSystem();

            var manager = new RuntimeSystemManager(new ISystem[] {oneComponentSystem}, Mock.Of<ILogger>());

            var oneComponentEntity = new EngineEntity(0, new IComponent[] {new SimpleComponent()},
                Mock.Of<IEventDispatcher>());

            manager.AddEntities(new[] {oneComponentEntity});
            manager.RemoveEntities(new[] {oneComponentEntity});

            manager.Systems.Single().Entities.ShouldNotContain(oneComponentEntity);
        }
    }
}
