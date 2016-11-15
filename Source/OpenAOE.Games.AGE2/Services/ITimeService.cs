namespace OpenAOE.Games.AGE2.Services
{
    public interface ITimeService
    {
        double CurrentTime { get; }

        double Step { get; }
    }
}
