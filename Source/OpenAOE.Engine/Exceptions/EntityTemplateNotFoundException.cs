using System;

namespace OpenAOE.Engine.Exceptions
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