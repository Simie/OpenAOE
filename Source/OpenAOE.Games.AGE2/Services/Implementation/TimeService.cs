using Ninject.Extensions.Logging;
using OpenAOE.Engine.System;

namespace OpenAOE.Games.AGE2.Implementation
{
    class TimeService : ITimeService
    {
        private readonly ILogger _logger;
        public double CurrentTime { get; private set; }

        public double Step
        {
            get { return 0.1; }
        }

        public TimeService(ILogger logger)
        {
            _logger = logger;
        }

        public void OnTick()
        {
            var newTime = CurrentTime + Step;
            //_logger.Trace($"Advancing CurrentTime to {newTime}");
            CurrentTime = newTime;
        }
    }
}