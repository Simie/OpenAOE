using OpenAOE.Engine.System;
using OpenAOE.Games.AGE2.Services.Implementation;

namespace OpenAOE.Games.AGE2.Systems
{
    /// <summary>
    /// Advances the simulation time at the start of the frame.
    /// </summary>
    internal class TimeSystem : ISystem, Triggers.IOnTick
    {
        public string Name => nameof(TimeService);

        private readonly TimeService _timeService;

        public TimeSystem(TimeService timeService)
        {
            _timeService = timeService;
        }

        public void OnTick()
        {
            _timeService.OnTick();
        }
    }
}
