using Ninject;
using NUnit.Framework;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Tests.TestData;
using OpenAOE.Engine.Tests.TestData.Components;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    class ComponentFactoryTests
    {
        private IKernel _kernel;
        private IComponentFactory _componentFactory;

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel(new EngineModule(), new TestComponentModule());
            _componentFactory = new ComponentFactory(_kernel);

            _componentFactory.ShouldNotBeNull();
        }

        [Test]
        public void TestFactoryReturnsCorrectInstanceType()
        {
            var component = _componentFactory.Create<ISimpleComponent>();

            component.ShouldNotBeNull();
            component.ShouldBeAssignableTo<ISimpleComponent>();
            component.ShouldBeOfType<SimpleComponent>();
        }

        [Test]
        public void TestFactoryReturnsCorrectInstanceTypeNonGeneric()
        {
            var component = _componentFactory.Create(typeof(ISimpleComponent));

            component.ShouldNotBeNull();
            component.ShouldBeAssignableTo<ISimpleComponent>();
            component.ShouldBeOfType<SimpleComponent>();
        }
    }
}
