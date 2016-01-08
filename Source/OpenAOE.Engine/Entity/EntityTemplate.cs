using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity
{
    public sealed class EntityTemplate
    {
        public string Key { get; }

        public IReadOnlyCollection<IComponent> Components { get; }

        public EntityTemplate(string key, IEnumerable<IComponent> components)
        {
            Key = key;
            Components = components.ToList();
        }
    }
}
