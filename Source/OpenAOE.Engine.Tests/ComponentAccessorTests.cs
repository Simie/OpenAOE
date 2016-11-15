using System;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Tests.TestData.Components.Bad;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class ComponentAccessorTests
    {
        [Test]
        public void ComponentAccessorFactoryRejectsNonComponent()
        {
            Should.Throw<ArgumentException>(() => new ComponentAccessor(typeof(IAmNotComponent)));
        }

        [Test]
        public void ComponentAccessorRejectsComponentInterface()
        {
            Should.Throw<ArgumentException>(() => { new ComponentAccessor(typeof(IComponent)); });
            Should.Throw<ArgumentException>(() => { new ComponentAccessor(typeof(IWriteableComponent)); });
            Should.Throw<ArgumentException>(
                () => { new ComponentAccessor(typeof(IWriteableComponent<ISimpleComponent>)); });
        }

        [Test]
        public void ComponentAccessorReturnsCorrectType()
        {
            ComponentMap<ISimpleComponent>.Accessor.ComponentType.ShouldBe(typeof(ISimpleComponent));
            WriteableComponentMap<IWriteableSimpleComponent>.Accessor.ComponentType.ShouldBe(typeof(ISimpleComponent));
        }

        [Test]
        public void DataAccessorRejectsObjectType()
        {
            Should.Throw<ArgumentException>(() => { new ComponentAccessor(typeof(object)); });
        }

        [Test]
        public void ReturnsUniqueIdForEachComponent()
        {
            var accessor1 = ComponentMap<ISimpleComponent>.Accessor;
            var accessor2 = ComponentMap<IOtherSimpleComponent>.Accessor;

            accessor1.Id.ShouldNotBe(accessor2.Id);
        }

        [Test]
        public void TestAccessorRejectsInvalidWriteableComponent()
        {
            Should.Throw<InvalidOperationException>(
                () => { new ComponentAccessor(typeof(IMultiInheritingWritableComponent)); });
        }

        [Test]
        public void TestEquality()
        {
            var a1 = new ComponentAccessor(typeof(ISimpleComponent));
            var a2 = new ComponentAccessor(typeof(ISimpleComponent));

            (a1 == a2).ShouldBeTrue();
        }

        [Test]
        public void TestInequality()
        {
            var a1 = new ComponentAccessor(typeof(ISimpleComponent));
            var a2 = new ComponentAccessor(typeof(IOtherSimpleComponent));

            (a1 != a2).ShouldBeTrue();
        }

        [Test]
        public void TestWriteableComponentReturnsIdForNonWriteableComponent()
        {
            var accessor1 = ComponentMap<ISimpleComponent>.Accessor;
            var accessor2 = WriteableComponentMap<IWriteableSimpleComponent>.Accessor;

            accessor1.ShouldBe(accessor2);
        }
    }
}
