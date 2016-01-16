using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return _templates[key];
        }
    }
}
