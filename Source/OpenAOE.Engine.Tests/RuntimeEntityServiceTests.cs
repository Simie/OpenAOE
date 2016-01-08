using System;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
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
