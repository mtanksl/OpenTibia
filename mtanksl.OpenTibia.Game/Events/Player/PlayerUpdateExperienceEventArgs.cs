using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateExperienceEventArgs : GameEventArgs
    {
        public PlayerUpdateExperienceEventArgs(Player player, uint experience, ushort level)
        {
            Player = player;

            Experience = experience;

            Level = level;
        }

        public Player Player { get; set; }

        public uint Experience { get; set; }

        public ushort Level { get; set; }
    }
}