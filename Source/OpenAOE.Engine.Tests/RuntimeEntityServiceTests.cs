using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class RuntimeEntityServiceTests
    {
        [Test]
        public void AddEntityDoesntAddToListBeforeCommit()
        {
            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(), Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);

            e.Entities.Should().NotContain(entity);
            e.AddedEntities.Should().Contain(entity);
        }

        [Test]
        public void AddEntityAddsEntityToListAfterCommit()
        {
            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(), Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);
            e.CommitAdded();

            e.Entities.Should().Contain(entity);
            e.AddedEntities.Should().NotContain(entity);
            e.GetEntity(entity.Id).Should().Be(entity);
        }

        [Test]
        public void RemoveEntityDoesntRemoveFromListBeforeCommit()
        {
            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(), Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);
            e.CommitAdded();
            e.RemoveEntity(entity);

            e.Entities.Should().Contain(entity);
        }

        [Test]
        public void RemoveEntityRemovesFromListAfterCommit()
        {
            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(), Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);
            e.CommitAdded();
            e.RemoveEntity(entity);
            e.CommitRemoved();

            e.Entities.Should().NotContain(entity);
            e.GetEntity(entity.Id).Should().BeNull();
        }

        [Test]
        public void RemoveEntityClearsListAfterCommit()
        {
            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(), Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);
            e.CommitAdded();
            e.RemoveEntity(entity);
            e.CommitRemoved();
            
            e.RemovedEntities.Should().BeEmpty();
        }
        
        [Test]
        public void AddEntityPostsEvent()
        {
            var mock = new Mock<IEventDispatcher>();
            
            var e = new RuntimeEntityService(mock.Object, Mock.Of<ILogger>());

            e.CreateEntity(new IComponent[0]);

            mock.Verify(p => p.Post(It.IsAny<EntityAdded>()));
        }

        [Test]
        public void RemoveEntityPostsEvent()
        {
            var mock = new Mock<IEventDispatcher>();
            
            var e = new RuntimeEntityService(mock.Object, Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);

            mock.Verify(p => p.Post(It.IsAny<EntityAdded>()));
        }

        [Test]
        public void EnsureAddEntityThrowsWhenGateLocked()
        {
            var mock = new Mock<IAccessGate>();
            mock.Setup(gate => gate.TryEnter()).Returns(false);

            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(),
                Mock.Of<ILogger>());
            e.AddEntityAccessGate = mock.Object;

            Action act = () => e.CreateEntity(new IComponent[0]);
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowsIfCreateEntityFromTemplateCalledWithNoTemplateProvider()
        {
            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(),
                Mock.Of<ILogger>());

            Action act = () => e.CreateEntity("Test");
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void CreatesEntityFromTemplateWithCorrectComponents()
        {
            var mock = new Mock<IEntityTemplateProvider>();
            mock.Setup((c) => c.Get("Test")).Returns(new EntityTemplate("Test", new IComponent[]
            {
                new SimpleComponent(), new OtherSimpleComponent()
            }));

            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(),
                Mock.Of<ILogger>(), mock.Object);

            var entity = e.CreateEntity("Test");

            entity.HasComponent<ISimpleComponent>().Should().BeTrue();
            entity.HasComponent<IOtherSimpleComponent>().Should().BeTrue();
        }

        [Test]
        public void CreatesEntityFromTemplateSetsCorrectPrototype()
        {
            var mock = new Mock<IEntityTemplateProvider>();
            mock.Setup((c) => c.Get("Test")).Returns(new EntityTemplate("Test", new IComponent[] {}));

            var e = new RuntimeEntityService(Mock.Of<IEventDispatcher>(),
                Mock.Of<ILogger>(), mock.Object);

            var entity = e.CreateEntity("Test");

            (entity as EngineEntity).Prototype.Should().Be("Test");
        }

        [Test]
        public void CheckIdProviderDoesntOverrideExistingId()
        {
            var entityService = new RuntimeEntityService(Mock.Of<IEventDispatcher>(),
                Mock.Of<ILogger>(),
                new RuntimeEntityTemplateProvider(new[] {new EntityTemplate("Test", new IComponent[0])}),
                new[]
                {
                    new EngineEntity(0, new IComponent[0], Mock.Of<IEventDispatcher>()),
                    new EngineEntity(1, new IComponent[0], Mock.Of<IEventDispatcher>()),
                    new EngineEntity(5, new IComponent[0], Mock.Of<IEventDispatcher>()),
                });

            // Create new entity
            entityService.CreateEntity("Test");

            // Commit the added entities
            entityService.CommitAdded();

            entityService.Entities.Select(p => p.Id).Should().OnlyHaveUniqueItems("Entity IDs need to be unique.");
        }
    }
}
