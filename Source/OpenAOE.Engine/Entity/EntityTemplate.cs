using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.Entity
{
    /// <summary>
    /// A prototype for creating an entity. Contains a collection of
    /// components populated with data for the newly constructed entity,
    /// and a key to identify this template.
    /// </summary>
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
