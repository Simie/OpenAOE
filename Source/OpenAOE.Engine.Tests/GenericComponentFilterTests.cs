using FluentAssertions;
using Moq;
using NUnit.Framework;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Tests.TestData.Components;
using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class GenericComponentFilterTests
    {
        [Test]
        public void FilterMatchesSingleComponent()
        {
            var componentMock = new Mock<IHasComponents>();
            componentMock.Setup(s => s.HasComponent<ISimpleComponent>()).Returns(true);

            var filter = new GenericComponentFilter<ISimpleComponent>();
            filter.Filter(componentMock.Object).Should().BeTrue();
        }

        [Test]
        public void FilterFailsSingleMissingComponent()
        {
            var componentMock = new Mock<IHasComponents>();
            componentMock.Setup(s => s.HasComponent<ISimpleComponent>()).Returns(false);

            var filter = new GenericComponentFilter<ISimpleComponent>();
            filter.Filter(componentMock.Object).Should().BeFalse();
        }

        [Test]
        public void FilterMatchesTwoComponents()
        {
            var componentMock = new Mock<IHasComponents>();
            componentMock.Setup(s => s.HasComponent<ISimpleComponent>()).Returns(true);
            componentMock.Setup(s => s.HasComponent<IOtherSimpleComponent>()).Returns(true);

            var filter = new GenericComponentFilter<ISimpleComponent, IOtherSimpleComponent>();
            filter.Filter(componentMock.Object).Should().BeTrue();
        }

        [Test]
        public void DoubleFilterFailsSingleMissingComponent()
        {
            var componentMock = new Mock<IHasComponents>();
            componentMock.Setup(s => s.HasComponent<ISimpleComponent>()).Returns(true);
            componentMock.Setup(s => s.HasComponent<IOtherSimpleComponent>()).Returns(false);

            var filter = new GenericComponentFilter<ISimpleComponent, IOtherSimpleComponent>();
            filter.Filter(componentMock.Object).Should().BeFalse();
        }

        [Test]
        public void DoubleFilterFailsTwoMissingComponents()
        {
            var componentMock = new Mock<IHasComponents>();
            componentMock.Setup(s => s.HasComponent<ISimpleComponent>()).Returns(false);
            componentMock.Setup(s => s.HasComponent<IOtherSimpleComponent>()).Returns(false);

            var filter = new GenericComponentFilter<ISimpleComponent, IOtherSimpleComponent>();
            filter.Filter(componentMock.Object).Should().BeFalse();
        }
    }
}
