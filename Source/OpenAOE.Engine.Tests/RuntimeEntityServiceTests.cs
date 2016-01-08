using System;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class RuntimeEntityServiceTests
    {
        [Test]
        public void AddEntityAddsEntityToListAfterCommit()
        {
            var e = new RuntimeEntityService(new UniqueIdProvider(), new AccessGate(), Mock.Of<IEventPoster>(), Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);

            e.Entities.ShouldNotContain(entity);
            e.AddedEntities.ShouldContain(entity);

            e.CommitAdded();

            e.Entities.ShouldContain(entity);
            e.AddedEntities.ShouldNotContain(entity);
            e.GetEntity(entity.Id).ShouldBe(entity);
        }

        [Test]
        public void RemoveEntityRemovesEntityFromListAfterCommit()
        {
            var e = new RuntimeEntityService(new UniqueIdProvider(), new AccessGate(), Mock.Of<IEventPoster>(), Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);
            e.CommitAdded();

            e.Entities.ShouldContain(entity);

            e.RemoveEntity(entity);

            e.RemovedEntities.ShouldContain(entity);
            e.Entities.ShouldContain(entity);

            e.CommitRemoved();

            e.Entities.ShouldNotContain(entity);
            e.RemovedEntities.ShouldNotContain(entity);
            e.GetEntity(entity.Id).ShouldBeNull();
        }

        [Test]
        public void AddEntityPostsEvent()
        {
            var mock = new Mock<IEventPoster>();
            
            var e = new RuntimeEntityService(new UniqueIdProvider(), new AccessGate(), mock.Object, Mock.Of<ILogger>());

            e.CreateEntity(new IComponent[0]);

            mock.Verify(p => p.Post(It.IsAny<EntityAdded>()));
        }

        [Test]
        public void RemoveEntityPostsEvent()
        {
            var mock = new Mock<IEventPoster>();
            
            var e = new RuntimeEntityService(new UniqueIdProvider(), new AccessGate(), mock.Object, Mock.Of<ILogger>());

            var entity = e.CreateEntity(new IComponent[0]);

            mock.Verify(p => p.Post(It.IsAny<EntityAdded>()));
        }

        [Test]
        public void EnsureAddEntityThrowsWhenGateLocked()
        {
            var mock = new Mock<IAccessGate>();
            mock.Setup(gate => gate.TryEnter()).Returns(false);

            var e = new RuntimeEntityService(new UniqueIdProvider(), mock.Object, Mock.Of<IEventPoster>(),
                Mock.Of<ILogger>());

            Should.Throw<InvalidOperationException>(() => e.CreateEntity("SomeEntity"));
        }
    }
}
