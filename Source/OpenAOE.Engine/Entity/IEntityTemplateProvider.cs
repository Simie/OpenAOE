namespace OpenAOE.Engine.Entity
{
    /// <summary>
    /// Provides access to EntityTemplate objects by key.
    /// </summary>
    public interface IEntityTemplateProvider
    {
        EntityTemplate Get(string key);
    }
}
