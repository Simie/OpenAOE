namespace OpenAOE.Engine.Data.Events
{
    public class EntityAdded : Event
    {
        public uint EntityId { get; }

        public EntityAdded(uint entityId)
        {
            EntityId = entityId;
        }
    }
}
