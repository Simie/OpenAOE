using System;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Tests.TestData.Components;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class ComponentInitializerTests
    {
        public void EnsureStaticInitalizerThrowsForInvalidComponentInterface()
        {
            Assert.Throws<NotSupportedException>(
                () => SimpleComponent.VerifyGenericParameters(typeof (IComponent), typeof (IWriteableSimpleComponent)));
        }

        public void EnsureStaticInitalizerThrowsForInvalidWriteableComponentInterface()
        {
            Assert.Throws<NotSupportedException>(
                () => SimpleComponent.VerifyGenericParameters(typeof (ISimpleComponent), typeof (IWriteableComponent)));
        }

        public void EnsureStaticInitializerDoesNotThrowForValidType()
        {
            Assert.DoesNotThrow(
                () =>
                    SimpleComponent.VerifyGenericParameters(typeof (ISimpleComponent),
                        typeof (IWriteableSimpleComponent)));
        }
    }
}
