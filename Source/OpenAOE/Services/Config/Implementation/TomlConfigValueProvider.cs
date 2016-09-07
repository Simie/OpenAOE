using System;
using Nett;

namespace OpenAOE.Services.Config.Implementation
{
    public class TomlConfigValueProvider : IConfigValueProvider
    {
        private readonly TomlTable _table;

        internal TomlConfigValueProvider(TomlTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            _table = table;
        }

        public TomlConfigValueProvider(string toml)
        {
            try {
                var table = Nett.Toml.ReadString(toml);
                _table = table;
            } catch (Exception e) {
                throw new ArgumentException("Error while reading TOML stream", e);
            }
        }

        public bool TryGetValue<T>(string category, string key, out T value)
        {
            var table = _table.TryGet<TomlTable>(category);
            var k = table?.TryGet<TomlObject>(key);
            if (k != null) {
                try
                {
                    value = k.Get<T>();
                    return true;
                }
                catch
                {
                    // ignored
                }
            }

            value = default(T);
            return false;
        }
    }
}