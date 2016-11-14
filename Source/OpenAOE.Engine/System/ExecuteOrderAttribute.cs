using System;

namespace OpenAOE.Engine.System
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ExecuteOrderAttribute : Attribute
    {
        public enum Positions
        {
            Before,
            After
        }

        public Type Other { get; }
        public Positions Position { get; }

        public ExecuteOrderAttribute(Type other, Positions position)
        {
            Other = other;
            Position = position;
        }
    }
}