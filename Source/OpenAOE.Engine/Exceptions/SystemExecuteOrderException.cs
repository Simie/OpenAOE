using System;

namespace OpenAOE.Engine.Exceptions
{
    public sealed class SystemExecuteOrderException : Exception
    {
        public SystemExecuteOrderException()
        {
        }

        public SystemExecuteOrderException(string message) : base(message)
        {
        }

        public SystemExecuteOrderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
