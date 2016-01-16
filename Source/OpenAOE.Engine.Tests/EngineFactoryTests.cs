using System.Collections.Generic;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Implementation;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class EngineFactoryTests : EngineTestsBase
    {
        [Test]
        public void CheckReturnsEngineInstance()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            engine.ShouldNotBeNull();
        }

        [Test]
        public void CheckReturnsDifferenceEngineInstances()
        {
            var engine1 = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var engine2 = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            engine1.ShouldNotBeSameAs(engine2);
        }

        [Test]
        public void CheckEnginesHaveDifferentServices()
        {
            var engine1 = (RuntimeEngine)Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var engine2 = (RuntimeEngine)Factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            engine1.EntityService.ShouldNotBeSameAs(engine2.EntityService);
        }
    }
}
