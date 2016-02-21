using Moq;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Exceptions;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void EntityContainsInitialComponents()
        {
            var entity = new Entity.EngineEntity(0, new IComponent[] {new SimpleComponent(), new OtherSimpleComponent()},
                Mock.Of<IEventDispatcher>());

            entity.HasComponent<ISimpleComponent>().ShouldBeTrue();
            entity.HasComponent<IOtherSimpleComponent>().ShouldBeTrue();
            entity.HasComponent<ISimpleAsyncComponent>().ShouldBeFalse();
        }

        [Test]
        public void EntityReturnsCorrectComponent()
        {
            var entity = new Entity.EngineEntity(0, new IComponent[] {new SimpleComponent()}, Mock.Of<IEventDispatcher>());

            var c = entity.Current<ISimpleComponent>();

            c.ShouldNotBeNull();
            c.ShouldBeOfType<SimpleComponent>();
        }

        [Test]
        public void EntityThrowsOnMultipleModifyCalls()
        {
            var entity = new Entity.EngineEntity(0, new IComponent[] {new SimpleComponent()}, Mock.Of<IEventDispatcher>());

            Should.NotThrow(() => entity.Modify<IWriteableSimpleComponent>());
            Should.Throw<ComponentAccessException>(() => entity.Modify<IWriteableSimpleComponent>());
        }

        [Test]
        public void EntityThrowsOnNonExistantAccess()
        {
            var entity = new Entity.EngineEntity(0, new IComponent[] {new SimpleComponent()}, Mock.Of<IEventDispatcher>());
            Should.Throw<ComponentAccessException>(() => entity.Current<IOtherSimpleComponent>());
        }

        [Test]
        public void EntityThrowsOnNonExistantModifyAccess()
        {
            var entity = new Entity.EngineEntity(0, new IComponent[] { new SimpleComponent() }, Mock.Of<IEventDispatcher>());
            Should.Throw<ComponentAccessException>(() => entity.Modify<IWriteableOtherSimpleComponent>());
        }

        [Test]
        public void EntityNotifiesPostsComponentChangeEvent()
        {
            var mock = new Mock<IEventDispatcher>();
            var entity = new Entity.EngineEntity(0, new IComponent[] {new SimpleComponent()}, mock.Object);

            entity.Modify<IWriteableSimpleComponent>();

            mock.Verify(poster => poster.Post(It.IsAny<EntityComponentModified>()), Times.AtLeastOnce);
        }

        [Test]
        public void EntityCommitsChangesToDirtyComponents()
        {
            var entity = new Entity.EngineEntity(0, new IComponent[] { new SimpleComponent() }, Mock.Of<IEventDispatcher>());

            var initialValue = entity.Current<ISimpleComponent>().Value;

            entity.Modify<IWriteableSimpleComponent>().Value = 5;
            entity.Current<ISimpleComponent>().Value.ShouldBe(initialValue);
            entity.Commit();
            entity.Current<ISimpleComponent>().Value.ShouldBe(5);
        }

        [Test]
        public void EntityCommitsDirtyComponentOnlyWhenDirty()
        {
            var mockWEntity = new Mock<IWriteableSimpleComponent>();
            var mockEntity = mockWEntity.As<ISimpleComponent>();

            var cloneWEntity = new Mock<IWriteableSimpleComponent>();
            var cloneEntity = cloneWEntity.As<ISimpleComponent>();

            mockEntity.SetupGet(c => c.Type).Returns(typeof (ISimpleComponent));
            mockEntity.Setup(c => c.Clone()).Returns(() => cloneEntity.Object);

            // Setup entity and mark ISimpleComponent as dirty
            var entity = new Entity.EngineEntity(0, new IComponent[] { mockEntity.Object }, Mock.Of<IEventDispatcher>());

            // Ensure a commit with the component non-dirty doesn't trigger a copy.
            entity.Commit();
            cloneEntity.Verify(c => c.CopyTo(mockEntity.Object),
                Times.Never());

            // Modify the component to set as dirty
            entity.Modify<IWriteableSimpleComponent>();

            // Trigger an entity commit, which should cause the entity to replicate
            // changes from `Next` to `Current`.
            entity.Commit();
            cloneEntity.Verify(c => c.CopyTo(mockEntity.Object),
                Times.Once());
        }
    }
}
