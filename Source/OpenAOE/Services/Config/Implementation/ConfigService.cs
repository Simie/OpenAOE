using System;
using System.Collections.Generic;
using Ninject.Extensions.Logging;

namespace OpenAOE.Services.Config.Implementation
{
    public class ConfigService : IConfigService
    {
        private readonly Dictionary<ConfigKey, Config> _configLookup = new Dictionary<ConfigKey, Config>();
        private readonly ILogger _logger;
        private readonly IConfigValueProvider _valueProvider;

        public ConfigService(ILogger logger, IConfigValueProvider valueProvider)
        {
            _logger = logger;
            _valueProvider = valueProvider;
        }

        public IConfig<T> GetConfig<T>(string category, string key, T defaultValue)
        {
            return GetConfig(true, category, key, defaultValue);
        }

        public IWriteableConfig<T> GetWritableConfig<T>(string category, string key, T defaultValue)
        {
            return GetConfig(false, category, key, defaultValue);
        }

        private Config<T> GetConfig<T>(bool isReadOnly, string category, string key, T defaultValue)
        {
            var lookupKey = new ConfigKey(category, key);

            Config existing;
            Config<T> e;
            if (!_configLookup.TryGetValue(lookupKey, out existing))
            {
                T value;
                if (!_valueProvider.TryGetValue(category, key, out value))
                    value = defaultValue;
                e = new Config<T>(value, defaultValue, isReadOnly);
                _configLookup.Add(lookupKey, e);
            }
            else
            {
                if (existing.Type != typeof(T))
                    throw new InvalidOperationException(
                        $"Config {category}.{key} has already been used with a different type.");
                e = (Config<T>) existing;
            }

            return e;
        }

        private struct ConfigKey
        {
            public readonly string Category;
            public readonly string Key;

            public ConfigKey(string category, string key)
            {
                Category = category;
                Key = key;
            }

            public bool Equals(ConfigKey other)
            {
                return string.Equals(Category, other.Category) && string.Equals(Key, other.Key);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is ConfigKey && Equals((ConfigKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Category != null ? Category.GetHashCode() : 0)*397) ^ (Key != null ? Key.GetHashCode() : 0);
                }
            }
        }

        private abstract class Config
        {
            public abstract Type Type { get; }
        }

        private class Config<T> : Config, IConfig<T>, IWriteableConfig<T>
        {
            public override Type Type => typeof(T);

            public bool IsReadOnly { get; }

            public T DefaultValue { get; }

            public T Value
            {
                get { return _value; }
                set
                {
                    if (IsReadOnly) throw new InvalidOperationException("Attempted to modify a read-only config.");
                    if (EqualityComparer<T>.Default.Equals(_value, value))
                        return;

                    var oldValue = _value;
                    _value = value;
                    Changed?.Invoke(this, new ConfigChangedEvent<T>(_value, oldValue));
                }
            }

            private T _value;

            public Config(T value, T defaultValue, bool isReadonly)
            {
                IsReadOnly = isReadonly;
                _value = value;
                DefaultValue = defaultValue;
            }

            public event ConfigChangedEventHandler<T> Changed;
        }
    }
}
