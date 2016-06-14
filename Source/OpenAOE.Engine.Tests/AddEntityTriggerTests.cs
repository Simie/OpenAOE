using Moq;
using Ninject;
using Ninject.MockingKernel;
using NUnit.Framework;
using OpenAOE.Engine.System;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class AddEntityTriggerTests
    {
        [Test]
        public void TestInitialEntitiesAreAdded()
        {
            var kernel = new MockingKernel(new EngineModule());

            var systemMock = new Mock<IEntitySystem>();
            systemMock.As<Triggers.IOnEntityAdded>();

            systemMock.Setup(p => p.Filter).Returns(ComponentFilter.Any);
            kernel.Bind<ISystem>().ToConstant(systemMock.Object);

            //var engine = kernel.Get<IEngineFactory>().Create();
        }
    }
}