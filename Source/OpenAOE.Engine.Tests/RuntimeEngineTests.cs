using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenAOE.Engine.Entity;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class RuntimeEngineTests : EngineTestsBase
    {
        [Test]
        public void TestThrowsExceptionWhenTickCalledDuringSync()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var t = engine.Tick(new EngineTickInput());
            t.Start();
            t.Wait();

            Should.Throw<InvalidOperationException>(() =>
            {
                var t2 = engine.Tick(new EngineTickInput());
                t2.Start();
                t2.Wait();
            });
        }

        [Test]
        public void TestThrowsExceptionWhenSyncCalledDuringTick()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());
            var t = engine.Tick(new EngineTickInput());

            Should.Throw<InvalidOperationException>(() =>
            {
                engine.Synchronize();
            });
        }

        [Test]
        public void TestDoesNotThrowDuringNormalOperation()
        {
            var engine = Factory.Create(new List<EntityData>(), new List<EntityTemplate>());

            var t = engine.Tick(new EngineTickInput());
            t.Start();
            t.Wait();

            engine.Synchronize();

            var t2 = engine.Tick(new EngineTickInput());
            t2.Start();
            t2.Wait();
        }
    }
}
