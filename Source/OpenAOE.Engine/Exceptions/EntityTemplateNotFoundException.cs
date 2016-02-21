using System;

namespace OpenAOE.Engine.Entity.Implementation
{
    public class EntityTemplateNotFoundException : Exception
    {
        public readonly string TemplateKey;

        public override string Message
        {
            get { return $"A template with key `{TemplateKey}` was not found."; }
        }

        public EntityTemplateNotFoundException(string key)
        {
            TemplateKey = key;
        }
    }
}