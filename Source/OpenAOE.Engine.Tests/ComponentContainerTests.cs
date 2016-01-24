using Moq;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity.Implementation;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class ComponentContainerTests
    {
        [Test]
        public void TestComponentContainerClonesInitialComponent()
        {
            var mock = new Mock<IComponent>();
            mock.Setup(c => c.Clone()).Returns(() => new Mock<IComponent>().Object);

            var componentContainer = new ComponentContainer(mock.Object);

            mock.Verify(c => c.Clone(), Times.Once());
        }

        [Test]
        public void TestComponentContainerReplicatesChanges()
        {
            var mock1 = new Mock<IComponent>();
            var mock2 = new Mock<IComponent>();

            mock1.Setup(c => c.Clone()).Returns(() => mock2.Object);

            var componentContainer = new ComponentContainer(mock1.Object);

            componentContainer.CommitChanges();

            mock2.Verify(c => c.CopyTo(mock1.Object), Times.Once());

            componentContainer.CommitChanges();

            mock2.Verify(c => c.CopyTo(mock1.Object), Times.Exactly(2));
        }

    }
}
