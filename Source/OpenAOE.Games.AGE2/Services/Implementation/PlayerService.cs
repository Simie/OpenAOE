using System;
using System.Collections.Generic;
using OpenAOE.Engine.Entity;
using OpenAOE.Games.AGE2.Data.Components;

namespace OpenAOE.Games.AGE2.Services.Implementation
{
    internal class PlayerService : IPlayerService
    {
        public IReadOnlyCollection<EngineEntity> Players => _playerEntities;

        private readonly List<EngineEntity> _playerEntities = new List<EngineEntity>();

        public void RegisterPlayerEntity(EngineEntity playerEntity)
        {
            if (!playerEntity.HasComponent<IPlayer>())
                throw new ArgumentException("Entity must have an IPlayer component.", nameof(playerEntity));

            _playerEntities.Add(playerEntity);
        }
    }
}
