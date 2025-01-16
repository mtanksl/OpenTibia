using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateMagicLevelEventArgs : GameEventArgs
    {
        public PlayerUpdateMagicLevelEventArgs(Player player, ulong magicLevelPoints, byte magicLevel)
        {
            Player = player;

            MagicLevelPoints = magicLevelPoints;

            MagicLevel = magicLevel;
        }

        public Player Player { get; }

        public ulong MagicLevelPoints { get; }

        public byte MagicLevel { get; }
    }
}