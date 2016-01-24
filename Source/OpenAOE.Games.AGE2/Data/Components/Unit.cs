using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public interface IUnit : IComponent {}

    public interface IWriteableUnit : IWriteableComponent {}

    class Unit : Component<Unit, IUnit, IWriteableUnit>, IUnit, IWriteableUnit
    {
        public FixVector2 Position { get; set; }

        public override void CopyTo(Unit other)
        {
            
        }
    }
}
