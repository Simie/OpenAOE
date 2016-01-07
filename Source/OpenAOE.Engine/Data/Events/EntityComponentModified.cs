using OpenAOE.Engine.Utility;

namespace OpenAOE.Engine.Data.Events
{
    public class EntityComponentModified : Event
    {
        public uint EntityId { get; }

        public ComponentAccessor ComponentAccessor { get; }

        public EntityComponentModified(uint entityId, ComponentAccessor componentAccessor)
        {
            EntityId = entityId;
            ComponentAccessor = componentAccessor;
        }
    }
}
