using System;
using FluentAssertions;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Entity;
using OpenAOE.Engine.Entity.Implementation;
using OpenAOE.Engine.Exceptions;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class RuntimeEntityTemplateProviderTests
    {
        [Test]
        public void PopulatesExceptionWhenTemplateNotFound()
        {
            var templateProvider = new RuntimeEntityTemplateProvider(new EntityTemplate[0]);
            var templateName = "NonExistantTemplate";

            Action act = () => templateProvider.Get(templateName);
            act.ShouldThrow<EntityTemplateNotFoundException>().And.TemplateKey.Should().Be(templateName);
        }

        [Test]
        public void ThrowsExceptionWhenTemplateNotFound()
        {
            var templateProvider = new RuntimeEntityTemplateProvider(new EntityTemplate[0]);
            Action act = () => templateProvider.Get("NonExistantTemplate");
            act.ShouldThrow<EntityTemplateNotFoundException>();
        }

        [Test]
        public void ThrowsOnDuplicateTemplateKey()
        {
            Action act = () =>
            {
                var templateProvider = new RuntimeEntityTemplateProvider(new[]
                {
                    new EntityTemplate("TestKey1", new IComponent[0]),
                    new EntityTemplate("TestKey1", new IComponent[0]),
                    new EntityTemplate("TestKey3", new IComponent[0])
                });
            };

            act.ShouldThrow<ArgumentException>();
        }
    }
}
