namespace OpenAOE.Services.Config
{
    public interface IConfigValueProvider
    {
        bool TryGetValue<T>(string category, string key, out T value);
    }
}