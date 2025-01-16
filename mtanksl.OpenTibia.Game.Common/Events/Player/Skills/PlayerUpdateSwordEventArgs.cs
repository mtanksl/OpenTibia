using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateSwordEventArgs : GameEventArgs
    {
        public PlayerUpdateSwordEventArgs(Player player, ulong swordPoints, byte sword)
        {
            Player = player;

            SwordPoints = swordPoints;

            Sword = sword;
        }

        public Player Player { get; }

        public ulong SwordPoints { get; }

        public byte Sword { get; }
    }
}