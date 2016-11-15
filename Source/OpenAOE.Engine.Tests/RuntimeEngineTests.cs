using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Modules;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;
using OpenAOE.Engine.Tests.TestData.Commands;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Tests.TestSystems;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class RuntimeEngineTests : EngineTestsBase
    {
        private class AddEntityEveryFrameSystem : FilteredSystem<ISimpleComponent>, Triggers.IOnEntityTick
        {
            private readonly IEntityService _entityService;

            public AddEntityEveryFrameSystem(IEntityService entityService)
            {
                _entityService = entityService;
            }

            public void OnTick(EngineEntity entity)
            {
                _entityService.CreateEntity("TestTemplate");
            }
        }

        private class RemoveEntityEveryFrameSystem : FilteredSystem<ISimpleComponent>, Triggers.IOnEntityTick
        {
            private readonly IEntityService _entityService;

            public RemoveEntityEveryFrameSystem(IEntityService entityService)
            {
                _entityService = entityService;
            }

            public void OnTick(EngineEntity entity)
            {
                _entityService.RemoveEntity(entity);
            }
        }

        private class AddEntityEveryFrameEngineModule : NinjectModule, IEngineModule
        {
            public override void Load()
            {
                Bind<ISystem>().To<AddEntityEveryFrameSystem>();
                Unbind<IEventDispatcher>();
            }
        }

        private class RemoveEntityEveryFrameEngineModule : NinjectModule, IEngineModule
        {
            public override void Load()
            {
                Bind<ISystem>().To<RemoveEntityEveryFrameSystem>();
                Unbind<IEventDispatcher>();
            }
        }

        private class ModifyEntityEveryTickSystemModule : NinjectModule, IEngineModule
        {
            public override void Load()
            {
                Bind<ISystem>().To<ModifyEntityEveryTickSystem>();
            }
        }

        [Test]
        public void CommandsAreExecuted()
        {
            var handlerMock = new Mock<Triggers.IOnCommand<TestCommand>>();
            var systemMock = handlerMock.As<ISystem>();

            var kernel = new StandardKernel(new NinjectSettings(), new EngineModule());
            kernel.Bind<ISystem>().ToConstant(systemMock.Object);
            kernel.Bind<ILogger>().ToConstant(Mock.Of<ILogger>());

            var engine = kernel.Get<IEngineFactory>().Create(new List<EntityData>(), new List<EntityTemplate>());

            var command = new TestCommand();
            // Execute the tick with a command
            var task = engine.Tick(new EngineTickInput(new List<Command> {command}));
            task.Start();
            task.Wait();

            engine.Synchronize();

            handlerMock.Verify(p => p.OnCommand(command));
        }

        [Test]
        public void ComponentDirtyFlagIsClearedEveryFrame()
        {
            var kernel = new StandardKernel(new NinjectSettings(), new EngineModule());
            kernel.Bind<IEngineModule>().To<ModifyEntityEveryTickSystemModule>();
            kernel.Bind<ILogger>().ToConstant(Mock.Of<ILogger>());

            // Create an engine with some test data set up to create a new entity every tick.
            var engine =
                kernel.Get<IEngineFactory>()
                    .Create(new List<EntityData> {new EntityData(0, new IComponent[] {new SimpleComponent()})},
                        new List<EntityTemplate>
                        {
                            new EntityTemplate("TestTemplate", new IComponent[] {new OtherSimpleComponent()})
                        });

            // Execute the tick
            var task = engine.Tick(new EngineTickInput());
            task.Start();
            task.Wait();

            engine.Synchronize();

            // Execute another tick
            var task2 = engine.Tick(new EngineTickInput());
            task2.Start();
            task2.Wait();
        }

        [Test]
        public void EntitiesAddedDuringFrameAreNotVisibleUntilSync()
        {
            var kernel = new StandardKernel(new NinjectSettings(), new EngineModule());
            kernel.Bind<IEngineModule>().To<AddEntityEveryFrameEngineModule>();
            kernel.Bind<ILogger>().ToConstant(Mock.Of<ILogger>());
            kernel.Bind<IEventDispatcher>().ToConstant(Mock.Of<IEventDispatcher>()).InSingletonScope();

            // Create an engine with some test data set up to create a new entity every tick.
            var engine =
                kernel.Get<IEngineFactory>()
                    .Create(new List<EntityData> {new EntityData(0, new IComponent[] {new SimpleComponent()})},
                        new List<EntityTemplate>
                        {
                            new EntityTemplate("TestTemplate", new IComponent[] {new OtherSimpleComponent()})
                        });

            // Execute a tick
            var task = engine.Tick(new EngineTickInput());
            task.Start();
            task.Wait();

            // Ensure entities are only added after the sync
            engine.Entities.Should().HaveCount(1);
            engine.Synchronize();
            engine.Entities.Should().HaveCount(2);
        }

        [Test]
        public void EntitiesAddedDuringFrameHaveEventsPosted()
        {
            var eventMocker = new Mock<IEventDispatcher>();

            var kernel = new StandardKernel(new NinjectSettings(), new EngineModule());
            kernel.Bind<IEngineModule>().To<AddEntityEveryFrameEngineModule>();

            kernel.Bind<IEventDispatcher>().ToConstant(eventMocker.Object).InSingletonScope();
            kernel.Bind<ILogger>().ToConstant(Mock.Of<ILogger>());

            // Create an engine with some test data set up to create a new entity every tick.
            var engine =
                kernel.Get<IEngineFactory>()
                    .Create(new List<EntityData> {new EntityData(0, new IComponent[] {new SimpleComponent()})},
                        new List<EntityTemplate>
                        {
                            new EntityTemplate("TestTemplate", new IComponent[] {new OtherSimpleComponent()})
                        });

            // Execute the tick
            var task = engine.Tick(new EngineTickInput());
            task.Start();
            task.Wait();

            eventMocker.Verify(p => p.Post(It.IsAny<EntityAdded>()), Times.AtLeastOnce,
                "EntityAdded event should be posted for each entity added");
        }

        [Test]
        public void EntitiesRemovedDuringFrameAreVisibleUntilNextTickStarts()
        {
            var kernel = new StandardKernel(new NinjectSettings(), new EngineModule());
            kernel.Bind<IEngineModule>().To<RemoveEntityEveryFrameEngineModule>();
            kernel.Bind<IEventDispatcher>().ToConstant(Mock.Of<IEventDispatcher>()).InSingletonScope();
            kernel.Bind<ILogger>().ToConstant(Mock.Of<ILogger>());

            // Create an engine with some test data set up to create a new entity every tick.
            var engine =
                kernel.Get<IEngineFactory>()
                    .Create(new List<EntityData>
                        {
                            new EntityData(0, new IComponent[] {new SimpleComponent()}),
                            new EntityData(1, new IComponent[] {new SimpleComponent()})
                        },
                        new List<EntityTemplate>());

            // Execute a tick
            var task = engine.Tick(new EngineTickInput());
            task.Start();
            task.Wait();
            engine.Synchronize();

            engine.Entities.Should().HaveCount(2, "removed entities should be visible until next tick begins");

            // Execute another tick
            task = engine.Tick(new EngineTickInput());
            task.Start();
            task.Wait();

            engine.Entities.Should()
                .BeEmpty("removed entities should be removed from entity list at start of next tick.");
        }

        [Test]
        public void EntitiesRemovedDuringFrameHaveEventsPosted()
        {
            var eventMocker = new Mock<IEventDispatcher>();

            var kernel = new StandardKernel(new NinjectSettings(), new EngineModule());
            kernel.Bind<IEngineModule>().To<RemoveEntityEveryFrameEngineModule>();

            kernel.Bind<IEventDispatcher>().ToConstant(eventMocker.Object).InSingletonScope();
            kernel.Bind<ILogger>().ToConstant(Mock.Of<ILogger>());

            // Create an engine with some test data set up to create a new entity every tick.
            var engine =
                kernel.Get<IEngineFactory>()
                    .Create(new List<EntityData>
                        {
                            new EntityData(0, new IComponent[] {new SimpleComponent()}),
                            new EntityData(1, new IComponent[] {new SimpleComponent()})
                        },
                        new List<EntityTemplate>());

            // Execute a tick
            var task = engine.Tick(new EngineTickInput());
            task.Start();
            task.Wait();
            engine.Synchronize();

            eventMocker.Verify(p => p.Post(It.IsAny<EntityRemoved>()), Times.Exactly(2),
                "EntityRemoved events were not posted when entities were removed");
        }

        [Test]
        public void TestDoesNotThrowDuringNormalOperation()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            var t = engine.Tick(new EngineTickInput());
            t.Start();
            t.Wait();

            engine.Synchronize();

            var t2 = engine.Tick(new EngineTickInput());
            t2.Start();
            t2.Wait();
        }

        [Test]
        public void TestThrowsExceptionWhenSyncCalledDuringTick()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var t = engine.Tick(new EngineTickInput());

            Action action = () => { engine.Synchronize(); };

            action.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void TestThrowsExceptionWhenTickCalledDuringSync()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var t = engine.Tick(new EngineTickInput());
            t.Start();
            t.Wait();

            Action action = () =>
            {
                var t2 = engine.Tick(new EngineTickInput());
                t2.Start();
                t2.Wait();
            };

            action.ShouldThrow<InvalidOperationException>();
        }
    }
}
