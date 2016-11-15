using System;

namespace OpenAOE.Engine.Exceptions
{
    public class EntityTemplateNotFoundException : Exception
    {
        public override string Message => $"A template with key `{TemplateKey}` was not found.";

        public readonly string TemplateKey;

        public EntityTemplateNotFoundException(string key)
        {
            TemplateKey = key;
        }
    }
}
