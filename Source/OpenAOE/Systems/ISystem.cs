namespace OpenAOE.Systems
{
    public interface ISystem
    {
        string Name { get; }

        void Tick();
    }
}
