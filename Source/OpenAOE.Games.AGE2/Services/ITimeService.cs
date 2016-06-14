namespace OpenAOE.Games.AGE2
{
    public interface ITimeService
    {
        double CurrentTime { get; }
        double Step { get; }
    }
}