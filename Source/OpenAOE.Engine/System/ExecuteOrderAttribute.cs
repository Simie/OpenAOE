using System;

namespace OpenAOE.Engine.System
{
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