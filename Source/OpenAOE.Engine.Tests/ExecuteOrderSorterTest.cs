using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenAOE.Engine.System;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    /// <summary>
    /// Tests for the <see cref="ExecuteOrderSorter"/> class.
    /// </summary>
    [TestFixture]
    class ExecuteOrderSorterTest
    {
        private class Test1
        {
            
        }

        [ExecuteOrder(typeof(Test1), ExecuteOrderAttribute.Positions.After)]
        private class AfterTest1
        {
            
        }

        [ExecuteOrder(typeof(Test1), ExecuteOrderAttribute.Positions.Before)]
        private class BeforeTest1
        {
            
        }

        [ExecuteOrder(typeof(BeforeTest1), ExecuteOrderAttribute.Positions.After)]
        private class AfterBeforeTest1
        {
            
        }

        private class Unspecified
        {
            
        }

        [Test]
        public void TestOrder()
        {
            var test1 = new Test1();
            var afterTest1 = new AfterTest1();
            var beforeTest1 = new BeforeTest1();
            var afterBeforeTest1 = new AfterBeforeTest1();
            var unspecified = new Unspecified();

            var list = new List<object>() {afterBeforeTest1, test1, beforeTest1, afterTest1, unspecified};
            var sorted = ExecuteOrderSorter.Sort(list);

            sorted.Count.ShouldBe(list.Count);

            foreach (var o in sorted)
            {
                Console.WriteLine(o);
            }

            AssertOrder(beforeTest1, test1, sorted);
            AssertOrder(test1, afterTest1, sorted);
            AssertOrder(beforeTest1, afterBeforeTest1, sorted);
        }

        void AssertOrder(object before, object after, IList<object> list)
        {
            list.ShouldContain(before);
            list.ShouldContain(after);

            foreach (var t in list)
            {
                if (t == before)
                {
                    return;
                }

                if (t == after)
                {
                    Assert.Fail($"Object `{after}` was before Object `{before}`");
                }
            }

            Assert.Fail($"List did not contain Object `{before}`");
        }
    }
}
