using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;

namespace OpenAOE.Engine.Tests.TestSystems
{
    public class AddEntityEveryTickSystem : ISystem, Triggers.IOnTick
    {
        public string Name => nameof(AddEntityEveryTickSystem);

        private readonly IEntityService _entityService;

        public AddEntityEveryTickSystem(IEntityService entityService)
        {
            _entityService = entityService;
        }

        public void OnTick()
        {
            _entityService.CreateEntity("Test");
        }
    }
}