using System.Collections.Generic;
using OpenAOE.Engine.Entity;

namespace OpenAOE.Games.AGE2.Services
{
    public interface IPlayerService
    {
        IReadOnlyCollection<EngineEntity> Players { get; }
    }
}