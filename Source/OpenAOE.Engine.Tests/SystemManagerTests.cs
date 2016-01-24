
using System.Linq;
using Moq;
using NUnit.Framework;
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
        public class TwoComponentSystem : ISystem
        {
            public IComponentFilter Filter => new GenericComponentFilter<ISimpleComponent, IOtherSimpleComponent>();
        }

        public class OneComponentSystem : ISystem
        {
            public IComponentFilter Filter => new GenericComponentFilter<ISimpleComponent>();
        }
        
        [Test]
        public void CreatesInstanceForEverySystemInConstructor()
        {
            var systems = new ISystem[]{new TwoComponentSystem(), new OneComponentSystem()};
            var manager = new RuntimeSystemManager(systems);

            manager.Systems.Count.ShouldBe(2);
            manager.Systems.ShouldAllBe(p => systems.Contains(p.System));
        }

        [Test]
        public void CorrectlyFilterEntitiesIntoSystemInstances()
        {
            var oneComponentSystem = new OneComponentSystem();
            var twoComponentSystem = new TwoComponentSystem();

            var manager = new RuntimeSystemManager(new ISystem[] {oneComponentSystem, twoComponentSystem});

            var oneComponentEntity = new Mock<IEntity>();
            oneComponentEntity.Setup(p => p.HasComponent<ISimpleComponent>()).Returns(true);

            var twoComponentEntity = new Mock<IEntity>();
            twoComponentEntity.Setup(p => p.HasComponent<ISimpleComponent>()).Returns(true);
            twoComponentEntity.Setup(p => p.HasComponent<IOtherSimpleComponent>()).Returns(true);

            manager.AddEntities(new[] {oneComponentEntity.Object, twoComponentEntity.Object});

            manager.Systems.Single(p => p.System == oneComponentSystem).Entities.ShouldContain(oneComponentEntity.Object);
            manager.Systems.Single(p => p.System == oneComponentSystem).Entities.ShouldContain(twoComponentEntity.Object);

            manager.Systems.Single(p => p.System == twoComponentSystem).Entities.ShouldContain(twoComponentEntity.Object);
            manager.Systems.Single(p => p.System == twoComponentSystem).Entities.ShouldNotContain(oneComponentEntity.Object);
        }


        [Test]
        public void RemovesEntityFromSystem()
        {
            var oneComponentSystem = new OneComponentSystem();

            var manager = new RuntimeSystemManager(new ISystem[] {oneComponentSystem});

            var oneComponentEntity = new Mock<IEntity>();
            oneComponentEntity.Setup(p => p.HasComponent<ISimpleComponent>()).Returns(true);

            manager.AddEntities(new[] {oneComponentEntity.Object});
            manager.RemoveEntities(new[] {oneComponentEntity.Object});

            manager.Systems.Single().Entities.ShouldNotContain(oneComponentEntity.Object);
        }
    }
}
