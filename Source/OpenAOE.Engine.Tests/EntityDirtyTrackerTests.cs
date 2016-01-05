
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

            dirtyTracker.IsDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeFalse();
            dirtyTracker.SetDirty(ComponentMap<ISimpleComponent>.Accessor);
            dirtyTracker.IsDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeTrue();
        }

        [Test]
        public void TestExceptionThrownAfterSecondSetDirtyCall()
        {
            var dirtyTracker = new EntityDirtyTracker();

            dirtyTracker.SetDirty(ComponentMap<ISimpleComponent>.Accessor);

            Should.Throw<InvalidOperationException>(() =>
            {
                dirtyTracker.SetDirty(ComponentMap<ISimpleComponent>.Accessor);
            });
        }
    }
}
