using System;
using System.Collections.Generic;
using Moq;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Ninject.Modules;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Data.Events;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class RuntimeEngineTests : EngineTestsBase
    {
        [Test]
        public void TestThrowsExceptionWhenTickCalledDuringSync()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var t = engine.Tick(new EngineTickInput());
            t.Start();
            t.Wait();

            Should.Throw<InvalidOperationException>(() =>
            {
                var t2 = engine.Tick(new EngineTickInput());
                t2.Start();
                t2.Wait();
            });
        }

        [Test]
        public void TestThrowsExceptionWhenSyncCalledDuringTick()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var t = engine.Tick(new EngineTickInput());

            Should.Throw<InvalidOperationException>(() =>
            {
                engine.Synchronize();
            });
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
        
        private class AddEntityEveryFrameSystem : FilteredSystem<TestData.Components.ISimpleComponent>, Triggers.IOnEntityTick
        {
            private readonly IEntityService _entityService;

            public AddEntityEveryFrameSystem(IEntityService entityService)
            {
                _entityService = entityService;
            }

            public void OnTick(IEntity entity)
            {
                _entityService.CreateEntity("TestTemplate");
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
                      .Create(new List<EntityData>() {new EntityData(0, new IComponent[] {new SimpleComponent()})},
                          new List<EntityTemplate>()
                          {
                              new EntityTemplate("TestTemplate", new IComponent[] {new OtherSimpleComponent()})
                          });

            var task = engine.Tick(new EngineTickInput());
            task.Start();
            task.Wait();

            engine.Entities.Count.ShouldBe(2);
            eventMocker.Verify(p => p.Post(It.IsAny<EntityAdded>()), Times.AtLeastOnce);
        }
    }
}
