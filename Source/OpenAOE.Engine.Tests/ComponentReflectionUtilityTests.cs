using System;
using NUnit.Framework;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class ComponentReflectionUtilityTests
    {
        [Test]
        public void TestAccessReadOnlyInterface()
        {
            ComponentReflectionUtility.GetReadOnlyComponentInterface(typeof(SimpleComponent))
                .ShouldBe(typeof(ISimpleComponent));
        }

        [Test]
        public void TestAccessWriteOnlyInterface()
        {
            ComponentReflectionUtility.GetWriteOnlyComponentInterface(typeof(SimpleComponent))
                .ShouldBe(typeof(IWriteableSimpleComponent));
        }

        [Test]
        public void TestInvalidTypeThrowsException()
        {
            Should.Throw<ArgumentException>(
                () =>
                        ComponentReflectionUtility.GetReadOnlyComponentInterface(typeof(ComponentReflectionUtilityTests)));
        }
    }
}
