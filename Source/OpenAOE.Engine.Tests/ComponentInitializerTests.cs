using System;
using NUnit.Framework;
using OpenAOE.Engine.Data;
using OpenAOE.Engine.Tests.TestData.Components;
using Shouldly;

namespace OpenAOE.Engine.Tests
{
    [TestFixture]
    public class ComponentInitializerTests
    {
        class InvalidComponentInterface : Component<InvalidComponentInterface, IComponent, IWriteableSimpleComponent>,
            IWriteableSimpleComponent
        {
            public override void CopyTo(InvalidComponentInterface other)
            {
                throw new NotImplementedException();
            }

            public int Value
            {
                set { throw new NotImplementedException(); }
            }
        }

        class InvalidWriteableComponentInterface : Component<InvalidWriteableComponentInterface, ISimpleComponent, IWriteableComponent>,
            ISimpleComponent, IWriteableComponent
        {
            public override void CopyTo(InvalidWriteableComponentInterface other)
            {
                throw new NotImplementedException();
            }

            public int Value
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void EnsureStaticInitalizerThrowsForInvalidComponentInterface()
        {
            Should.Throw<TypeInitializationException>(() =>
            {
                new InvalidComponentInterface();
            });
        }

        public void EnsureStaticInitalizerThrowsForInvalidWriteableComponentInterface()
        {
            Should.Throw<TypeInitializationException>(() =>
            {
                new InvalidWriteableComponentInterface();
            });
        }

        public void EnsureStaticInitializerDoesNotThrowForValidType()
        {
            // TODO: This wouldn't throw anyway if SimpleComponent has already been used in another unit test this session.
            // But, it would fail that unit test so it should be fine...
            Should.NotThrow(() => new SimpleComponent());
        }
    }
}
