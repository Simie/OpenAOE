using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public interface IPlayer : IComponent
    {
        string Name { get; }

        byte Team { get; }

        byte ColourIndex { get; }
    }

    public interface IWriteablePlayer : IWriteableComponent<IPlayer>
    {
        string Name { set; }

        byte Team { set; }

        byte ColourIndex { set; }
    }

    internal class Player : Component<Player, IPlayer, IWriteablePlayer>, IPlayer, IWriteablePlayer
    {
        /// <summary>
        /// Name displayed in UI to refer to this player.
        /// </summary>
        public string Name { get; set; }

        public byte Team { get; set; }

        public byte ColourIndex { get; set; }

        public override void CopyTo(Player other)
        {
            other.Name = Name;
            other.Team = Team;
            other.ColourIndex = ColourIndex;
        }
    }
}
