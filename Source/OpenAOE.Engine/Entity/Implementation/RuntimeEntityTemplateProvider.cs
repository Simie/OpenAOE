using System.Collections.Generic;
using System.Linq;
using OpenAOE.Engine.Exceptions;

namespace OpenAOE.Engine.Entity.Implementation
{
    internal sealed class RuntimeEntityTemplateProvider : IEntityTemplateProvider
    {
        private readonly Dictionary<string, EntityTemplate> _templates;

        public RuntimeEntityTemplateProvider(IReadOnlyCollection<EntityTemplate> templates)
        {
            _templates = templates.ToDictionary(t => t.Key, t => t);
        }

        public EntityTemplate Get(string key)
        {
            EntityTemplate template;
            if (_templates.TryGetValue(key, out template))
                return template;

            throw new EntityTemplateNotFoundException(key);
        }
    }
}
