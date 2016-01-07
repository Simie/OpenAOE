using Moq;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Exceptions;
using OpenAOE.Engine.Tests.TestData.Components;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class RuntimeEntityTests
    {
        [Test]
        public void EntityContainsInitialComponents()
        {
            var entity = new RuntimeEntity(0, new IComponent[] {new SimpleComponent(), new OtherSimpleComponent()},
                Mock.Of<IEventPoster>());

            entity.HasComponent<ISimpleComponent>().ShouldBeTrue();
            entity.HasComponent<IOtherSimpleComponent>().ShouldBeTrue();
            entity.HasComponent<ISimpleAsyncComponent>().ShouldBeFalse();
        }

        [Test]
        public void EntityReturnsCorrectComponent()
        {
            var entity = new RuntimeEntity(0, new IComponent[] {new SimpleComponent()}, Mock.Of<IEventPoster>());

            var c = entity.Current<ISimpleComponent>();

            c.ShouldNotBeNull();
            c.ShouldBeOfType<SimpleComponent>();
        }

        [Test]
        public void EntityThrowsOnMultipleModifyCalls()
        {
            var entity = new RuntimeEntity(0, new IComponent[] {new SimpleComponent()}, Mock.Of<IEventPoster>());

            Should.NotThrow(() => entity.Modify<IWriteableSimpleComponent>());
            Should.Throw<ComponentAccessException>(() => entity.Modify<IWriteableSimpleComponent>());
        }

        [Test]
        public void EntityThrowsOnNonExistantAccess()
        {
            var entity = new RuntimeEntity(0, new IComponent[] {new SimpleComponent()}, Mock.Of<IEventPoster>());
            Should.Throw<ComponentAccessException>(() => entity.Current<IOtherSimpleComponent>());
        }

        [Test]
        public void EntityThrowsOnNonExistantModifyAccess()
        {
            var entity = new RuntimeEntity(0, new IComponent[] { new SimpleComponent() }, Mock.Of<IEventPoster>());
            Should.Throw<ComponentAccessException>(() => entity.Modify<IWriteableOtherSimpleComponent>());
        }

        [Test]
        public void EntityNotifiesPostsComponentChangeEvent()
        {
            var mock = new Mock<IEventPoster>();
            var entity = new RuntimeEntity(0, new IComponent[] {new SimpleComponent()}, mock.Object);

            entity.Modify<IWriteableSimpleComponent>();

            mock.Verify(poster => poster.Post(It.IsAny<EntityComponentModified>()), Times.AtLeastOnce);
        }

        [Test]
        public void EntityCommitsChangesToDirtyComponents()
        {
            var entity = new RuntimeEntity(0, new IComponent[] { new SimpleComponent() }, Mock.Of<IEventPoster>());

            var initialValue = entity.Current<ISimpleComponent>().Value;

            entity.Modify<IWriteableSimpleComponent>().Value = 5;
            entity.Current<ISimpleComponent>().Value.ShouldBe(initialValue);
            entity.Commit();
            entity.Current<ISimpleComponent>().Value.ShouldBe(5);
        }
    }
}
