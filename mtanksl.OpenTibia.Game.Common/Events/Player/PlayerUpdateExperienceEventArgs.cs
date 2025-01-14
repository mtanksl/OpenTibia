using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateExperienceEventArgs : GameEventArgs
    {
        public PlayerUpdateExperienceEventArgs(Player player, ulong experience, ushort level)
        {
            Player = player;

            Experience = experience;

            Level = level;
        }

        public Player Player { get; }

        public ulong Experience { get; }

        public ushort Level { get; }
    }
}