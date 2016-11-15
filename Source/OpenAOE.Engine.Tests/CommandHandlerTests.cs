using System;
using FluentAssertions;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.System;
using OpenAOE.Engine.System.Implementation;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class CommandHandlerTests
    {
        private class TestCommand1 : Command
        {
        }

        private class TestCommand2 : Command
        {
        }

        private class CommandSystem : ISystem, Triggers.IOnCommand<TestCommand1>, Triggers.IOnCommand<TestCommand2>
        {
            public string Name
            {
                get { throw new NotImplementedException(); }
            }

            public void OnCommand(TestCommand1 command)
            {
                throw new NotImplementedException();
            }

            public void OnCommand(TestCommand2 command)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void TestCommandHandlersAreDetected()
        {
            var sys = new CommandSystem();

            CommandHandlerUtil.GetCommandHandlers(sys)
                .Should()
                .HaveCount(2)
                .And.Contain(handler => handler.CanHandle(new TestCommand1()))
                .And.Contain(handler => handler.CanHandle(new TestCommand2()));
        }
    }
}
