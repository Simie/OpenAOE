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
        public void TestResetAllComponents()
        {
            var dirtyTracker = new EntityDirtyTracker();

            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeTrue();
            dirtyTracker.TrySetDirty(ComponentMap<IOtherSimpleComponent>.Accessor).ShouldBeTrue();

            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeFalse();
            dirtyTracker.TrySetDirty(ComponentMap<IOtherSimpleComponent>.Accessor).ShouldBeFalse();

            dirtyTracker.Reset();

            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeTrue();
            dirtyTracker.TrySetDirty(ComponentMap<IOtherSimpleComponent>.Accessor).ShouldBeTrue();
        }

        [Test]
        public void TestResetSingleComponent()
        {
            var dirtyTracker = new EntityDirtyTracker();

            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeTrue();
            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeFalse();

            dirtyTracker.Reset(ComponentMap<ISimpleComponent>.Accessor);

            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeTrue();
        }

        [Test]
        public void TrySetDirtyShouldReturnFalseAfterFirstCall()
        {
            var dirtyTracker = new EntityDirtyTracker();

            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeTrue();
            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeFalse();
            dirtyTracker.TrySetDirty(ComponentMap<ISimpleComponent>.Accessor).ShouldBeFalse();
        }
    }
}
