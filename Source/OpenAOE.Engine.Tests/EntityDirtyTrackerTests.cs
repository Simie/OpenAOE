
using System;
using NUnit.Framework;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class EntityDirtyTrackerTests
    {
        [Test]
        public void TestIsDirtyReturnsTrueAfterSetDirtyCall()
        {
            var dirtyTracker = new EntityDirtyTracker();

            dirtyTracker.IsDirty<ISimpleComponent>().ShouldBeFalse();
            dirtyTracker.SetDirty<ISimpleComponent>();
            dirtyTracker.IsDirty<ISimpleComponent>().ShouldBeTrue();
        }

        [Test]
        public void TestExceptionThrownAfterSecondSetDirtyCall()
        {
            var dirtyTracker = new EntityDirtyTracker();

            dirtyTracker.SetDirty<ISimpleComponent>();

            Should.Throw<InvalidOperationException>(() =>
            {
                dirtyTracker.SetDirty<ISimpleComponent>();
            });
        }

        /*[Test]
        public void TestUsingWriteableComponentThrowsException()
        {
            var dirtyTracker = new EntityDirtyTracker();

            Should.Throw<Exception>(() =>
            {
                dirtyTracker.SetDirty<IWriteableSimpleComponent>();
            });
        }*/
    }
}
