using FluentAssertions;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using OpenAOE.Services.Config;
using OpenAOE.Services.Config.Implementation;

namespace OpenAOE.Tests
{
    [TestFixture]
    public class ConfigServiceTests
    {
        [SetUp]
        public void Setup()
        {
            _configService = new ConfigService(Mock.Of<ILogger>(),
                new TomlConfigValueProvider(@"[TestCategory]
TestString=""Value""
TestInt=2434
[SecondTestCategory]
TestString=""SecondValue""
[TestDefaultValue]
Overridden=10
"));
        }

        private ConfigService _configService;

        [Test]
        public void ChangedValuesPropogate()
        {
            var mutable = _configService.GetWritableConfig("TestMutable", "TestKey2", "Default Value");
            var test = _configService.GetWritableConfig("TestMutable", "TestKey2", "Default Value");

            mutable.Value.Should().Be(test.Value, "they are the same config.");
            mutable.Value = "Changed Value";
            test.Value.Should().Be(mutable.Value).And.Be("Changed Value");
        }

        [Test]
        public void IncorrectTypeUsesDefaultValue()
        {
            var config = _configService.GetConfig("TestCategory", "TestString", 50);
            config.Value.Should().Be(50);
        }

        [Test]
        public void SimilarKeysDontConflict()
        {
            var value = _configService.GetConfig("TestCategory", "TestString", "Non-default Value");
            var value2 = _configService.GetConfig("SecondTestCategory", "TestString", "Non-default Value");

            value.Value.Should().NotBe(value2.Value, "they are in different categories.");
        }

        [Test]
        public void TestChangedEvent()
        {
            var mutable = _configService.GetWritableConfig("TestMutable", "TestKey", "Default Value");
            mutable.MonitorEvents();

            mutable.Value = "Non-default Value";
            mutable.ShouldRaise(nameof(mutable.Changed))
                .WithSender(mutable)
                .WithArgs<ConfigChangedEvent<string>>(
                    e => (e.NewValue == "Non-default Value") && (e.OldValue == "Default Value"));
        }

        [Test]
        public void TestDefaultValue()
        {
            var mutable = _configService.GetWritableConfig("TestDefaultValue", "TestKey", 50);
            mutable.Value.Should().Be(50, "because it is the default value.");
        }

        [Test]
        public void TestOverridenDefaultValue()
        {
            var mutable = _configService.GetWritableConfig("TestDefaultValue", "Overridden", 50);
            mutable.Value.Should().Be(10, "because it is overriden in the config string.");
        }

        [Test]
        public void TestReadString()
        {
            var value = _configService.GetConfig("TestCategory", "TestString", "Non-default Value");
            value.Value.Should().Be("Value", "it is defined in the config string");
        }
    }
}
