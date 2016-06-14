using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;
using OpenAOE.Engine.System.Implementation;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class SystemUpdateSchedulerTests
    {
        public class SystemFake : ISystem
        {
            public string Name => nameof(SystemFake);
            public IComponentFilter Filter
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class SystemInstanceFake : ISystemInstance
        {
            public ISystem System { get; } = new SystemFake();

            public IReadOnlyList<EngineEntity> Entities
            {
                get { throw new NotImplementedException(); }
            }

            public bool HasEntityTick
            {
                get { throw new NotImplementedException(); }
            }

            public bool HasEntityAdd
            {
                get { throw new NotImplementedException(); }
            }

            public bool HasEntityRemove
            {
                get { throw new NotImplementedException(); }
            }

            public bool HasTick
            {
                get { throw new NotImplementedException(); }
            }
        }

        [Test]
        public void SchedulerContainsAllSystems()
        {
            var systems = new List<ISystemInstance>()
            {
                new SystemInstanceFake(),
                new SystemInstanceFake(),
                new SystemInstanceFake(),
                new SystemInstanceFake(),
            };

            var updateScheduler = new SystemUpdateScheduler(systems);

            var scheduledSystems = updateScheduler.UpdateBursts.Single().Systems;
            scheduledSystems.Should().HaveSameCount(systems).And.Contain(systems);
        }
    }
}
