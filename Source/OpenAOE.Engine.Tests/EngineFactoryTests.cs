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
    public class EngineFactoryTests
    {
        private IKernel _kernel = new MoqMockingKernel(new NinjectSettings(), new EngineModule());

        private IEngineFactory _factory;

        public EngineFactoryTests()
        {
            _kernel.Bind<ILogger>().ToMock();
            _kernel.Bind<IEntityTemplateProvider>().ToMock();
            _factory = _kernel.Get<IEngineFactory>();
        }

        [Test]
        public void CheckReturnsEngineInstance()
        {
            var engine = _factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            engine.ShouldNotBeNull();
        }

        [Test]
        public void CheckReturnsDifferenceEngineInstances()
        {
            var engine1 = _factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var engine2 = _factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            engine1.ShouldNotBeSameAs(engine2);
        }

        [Test]
        public void CheckEnginesHaveDifferentServices()
        {
            var engine1 = (RuntimeEngine)_factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var engine2 = (RuntimeEngine)_factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            engine1.EntityService.ShouldNotBeSameAs(engine2.EntityService);
        }
    }
}
