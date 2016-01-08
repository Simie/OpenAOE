using System;
using NUnit.Framework;
using OpenAOE.Engine.Utility;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class AccessGateTests
    {
        [Test]
        public void TryEnterReturnsFalseWhenLocked()
        {
            var gate = new AccessGate {IsLocked = true};
            gate.TryEnter().ShouldBeFalse();
        }

        [Test]
        public void TryEnterReturnsTrueWhenUnlocked()
        {
            var gate = new AccessGate {IsLocked = false};
            gate.TryEnter().ShouldBeTrue();
        }

        [Test]
        public void GateThrowsWhenLocked()
        {
            var gate = new AccessGate {IsLocked = true};
            Should.Throw<InvalidOperationException>(() => gate.Enter());
        }

        [Test]
        public void GateDoesNotThrowWhenUnlocked()
        {
            var gate = new AccessGate {IsLocked = false};
            Should.NotThrow(() => gate.Enter());
        }
    }
}
