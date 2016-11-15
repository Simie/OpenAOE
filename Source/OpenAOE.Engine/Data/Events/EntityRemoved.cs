namespace OpenAOE.Engine.Data.Events
{
    public class EntityRemoved : Event
    {
        public uint EntityId { get; }

        public EntityRemoved(uint entityId)
        {
            EntityId = entityId;
        }
    }
}
