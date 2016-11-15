using OpenAOE.Engine.Entity;
using OpenAOE.Engine.System;
using OpenAOE.Games.AGE2.Data.Components;
using OpenAOE.Games.AGE2.Services.Implementation;

namespace OpenAOE.Games.AGE2.Systems
{
    /// <summary>
    /// System for registering player entities to the player service.
    /// </summary>
    internal class PlayerSystem : FilteredSystem<IPlayer>, Triggers.IOnEntityAdded
    {
        private readonly PlayerService _playerService;

        public PlayerSystem(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public void OnEntityAdded(EngineEntity entity)
        {
            _playerService.RegisterPlayerEntity(entity);
        }
    }
}
