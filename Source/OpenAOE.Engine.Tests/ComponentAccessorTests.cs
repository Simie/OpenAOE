using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void ReturnsUniqueIdForEachComponent()
        {
            var accessor1 = ComponentMap<ISimpleComponent>.Accessor;
            var accessor2 = ComponentMap<IOtherSimpleComponent>.Accessor;

            accessor1.Id.ShouldNotBe(accessor2.Id);
        }

        [Test]
        public void TestWriteableComponentReturnsIdForNonWriteableComponent()
        {
            var accessor1 = ComponentMap<ISimpleComponent>.Accessor;
            var accessor2 = WriteableComponentMap<IWriteableSimpleComponent>.Accessor;

            accessor1.ShouldBe(accessor2);
        }

        [Test]
        public void TestAccessorRejectsInvalidWriteableComponent()
        {
            Should.Throw<InvalidOperationException>(() =>
            {
                new ComponentAccessor(typeof (IMultiInheritingWritableComponent));
            });
        }

        [Test]
        public void DataAccessorRejectsObjectType()
        {
            Should.Throw<ArgumentException>(() =>
            {
                new ComponentAccessor(typeof (object));
            });
        }

        [Test]
        public void ComponentAccessorRejectsComponentInterface()
        {
            Should.Throw<ArgumentException>(() => 
            {
                new ComponentAccessor(typeof (IComponent));
            });
            Should.Throw<ArgumentException>(() => 
            {
                new ComponentAccessor(typeof (IWriteableComponent));
            });
            Should.Throw<ArgumentException>(() => 
            {
                new ComponentAccessor(typeof (IWriteableComponent<ISimpleComponent>));
            });
        }

        [Test]
        public void ComponentAccessorReturnsCorrectType()
        {
            ComponentMap<ISimpleComponent>.Accessor.ComponentType.ShouldBe(typeof (ISimpleComponent));
            WriteableComponentMap<IWriteableSimpleComponent>.Accessor.ComponentType.ShouldBe(typeof (ISimpleComponent));
        }

        [Test]
        public void ComponentAccessorFactoryRejectsNonComponent()
        {
            Should.Throw<ArgumentException>(() => new ComponentAccessor(typeof (IAmNotComponent)));
        }
    }
}
