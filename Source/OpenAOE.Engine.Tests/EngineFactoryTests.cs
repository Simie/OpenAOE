using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Implementation;
using OpenAOE.Engine.Tests.TestData.Components;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class EngineFactoryTests : EngineTestsBase
    {
        [Test]
        public void CheckEnginesHaveDifferentServices()
        {
            var engine1 = (RuntimeEngine) Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var engine2 = (RuntimeEngine) Factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            engine1.EntityService.ShouldNotBeSameAs(engine2.EntityService);
        }

        [Test]
        public void CheckReturnsDifferenceEngineInstances()
        {
            var engine1 = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var engine2 = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            engine1.ShouldNotBeSameAs(engine2);
        }

        [Test]
        public void CheckReturnsEngineInstance()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            engine.ShouldNotBeNull();
        }

        [Test]
        public void EnsureSnapshotEntitiesLoad()
        {
            var entitySnapshot = new List<EntityData>
            {
                new EntityData(0, new IComponent[] {new SimpleComponent(), new OtherSimpleComponent()}),
                new EntityData(1, new IComponent[] {})
            };

            var engine = Factory.Create(entitySnapshot, new List<EntityTemplate>());

            engine.Entities.Count.ShouldBe(2);
            engine.Entities.Any(
                    p => (p.Id == 0) && p.HasComponent<ISimpleComponent>() && p.HasComponent<IOtherSimpleComponent>())
                .ShouldBeTrue();
            engine.Entities.Any(
                    p => (p.Id == 1) && !p.HasComponent<ISimpleComponent>() && !p.HasComponent<IOtherSimpleComponent>())
                .ShouldBeTrue();
        }

        [Test]
        public void EnsureTemplateEntitiesLoad()
        {
            var templates = new List<EntityTemplate>
            {
                new EntityTemplate("Test1", new IComponent[] {new SimpleComponent(), new OtherSimpleComponent()}),
                new EntityTemplate("Test2", new IComponent[] {})
            };

            var engine = (RuntimeEngine) Factory.Create(new List<EntityData>(), templates);

            Should.NotThrow(() =>
            {
                var t1 = engine.EntityService.TemplateProvider.Get("Test1");

                t1.ShouldNotBeNull();
                t1.Components.Any(p => p is ISimpleComponent).ShouldBeTrue();
                t1.Components.Any(p => p is IOtherSimpleComponent).ShouldBeTrue();

                engine.EntityService.TemplateProvider.Get("Test2").ShouldNotBeNull();
            });
        }
    }
}
