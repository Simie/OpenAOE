using System;

namespace OpenAOE.Engine.Exceptions
{
    public class ComponentAccessException : Exception
    {
        public enum Reasons
        {
            ComponentAlreadyAccessed,
            ComponentDoesNotExist
        }

        public Type ComponentType { get; }
        public uint EntityId { get; }
        public Reasons Reason { get; }

        public override string Message
        {
            get
            {
                switch (Reason)
                {
                    case Reasons.ComponentAlreadyAccessed:
                        return
                            $"Component of type `{ComponentType}` on entity `{EntityId}` has already been accessed during this tick.";
                    case Reasons.ComponentDoesNotExist:
                        return $"Component of type `{ComponentType}` on entity `{EntityId}` does not exist.";
                }
                return base.Message;
            }
        }

        public ComponentAccessException(uint entityId, Type componentType, Reasons reason)
        {
            EntityId = entityId;
            ComponentType = componentType;
            Reason = reason;
        }
    }
}
