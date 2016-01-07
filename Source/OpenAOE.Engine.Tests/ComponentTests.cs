using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Tests.TestData.Components;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class ComponentTests
    {
        [Test]
        public void ComponentThrowsWhenCopyingToDifferentComponentType()
        {
            IComponent c1 = new SimpleComponent();
            IComponent c2 = new OtherSimpleComponent();

            Should.Throw<ArgumentException>(() =>
            {
                c1.CopyTo(c2);
            });
        }
    }
}
