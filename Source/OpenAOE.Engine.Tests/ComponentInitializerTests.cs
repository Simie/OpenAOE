using System;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Tests.TestData.Components;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class ComponentInitializerTests
    {
        [Test]
        public void EnsureStaticInitalizerThrowsForInvalidComponentInterface()
        {
            Assert.Throws<NotSupportedException>(
                () => SimpleComponent.VerifyGenericParameters(typeof(IComponent), typeof(IWriteableSimpleComponent)));
        }

        [Test]
        public void EnsureStaticInitalizerThrowsForInvalidWriteableComponentInterface()
        {
            Assert.Throws<NotSupportedException>(
                () => SimpleComponent.VerifyGenericParameters(typeof(ISimpleComponent), typeof(IWriteableComponent)));
        }

        [Test]
        public void EnsureStaticInitializerDoesNotThrowForValidType()
        {
            Assert.DoesNotThrow(
                () =>
                    SimpleComponent.VerifyGenericParameters(typeof(ISimpleComponent),
                        typeof(IWriteableSimpleComponent)));
        }
    }
}
