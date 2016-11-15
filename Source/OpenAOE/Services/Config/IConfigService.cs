using System;

namespace OpenAOE.Services.Config
{
    public delegate void ConfigChangedEventHandler<T>(IWriteableConfig<T> sender, ConfigChangedEvent<T> args);

    public sealed class ConfigChangedEvent<T> : EventArgs
    {
        public T OldValue { get; }

        public T NewValue { get; }

        public ConfigChangedEvent(T newValue, T oldValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public interface IConfig<T>
    {
        T Value { get; }
    }

    public interface IWriteableConfig<T> : IConfig<T>
    {
        new T Value { get; set; }

        event ConfigChangedEventHandler<T> Changed;
    }

    public interface IConfigService
    {
        IConfig<T> GetConfig<T>(string category, string key, T defaultValue);
        IWriteableConfig<T> GetWritableConfig<T>(string category, string key, T defaultValue);
    }
}
