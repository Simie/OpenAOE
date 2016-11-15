using Ninject.Extensions.Logging;

namespace OpenAOE.Games.AGE2.Services.Implementation
{
    internal class TimeService : ITimeService
    {
        public double CurrentTime { get; private set; }

        public double Step => 0.1;

        private readonly ILogger _logger;

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
