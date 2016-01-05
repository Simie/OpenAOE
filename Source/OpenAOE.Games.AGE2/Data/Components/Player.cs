using OpenAOE.Engine.Data;

namespace OpenAOE.Games.AGE2.Data.Components
{
    public class Player : Component<Player>
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
