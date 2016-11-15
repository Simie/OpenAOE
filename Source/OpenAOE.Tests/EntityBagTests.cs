using Moq;
using NUnit.Framework;
using OpenAOE.Services;

namespace OpenAOE.Tests
{
    [TestFixture]
    public class EntityBagTests
    {
        public void ClearsWhenEngineChanged()
        {
            var engineService = new Mock<IGameEngineService>();
        }
    }
}
