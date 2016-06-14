using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;
using OpenAOE.Engine.Tests.TestData.Components;

namespace OpenAOE.Engine.Tests.TestSystems
{
    public class ModifyEntityEveryTickSystem : FilteredSystem<ISimpleComponent>, Triggers.IOnEntityTick
    {
        public string Name => nameof(ModifyEntityEveryTickSystem);

        public void OnTick(EngineEntity entity)
        {
            entity.Modify<IWriteableSimpleComponent>();
        }
    }
}