using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateShieldEventArgs : GameEventArgs
    {
        public PlayerUpdateShieldEventArgs(Player player, ulong shieldPoints, byte shield)
        {
            Player = player;

            ShieldPoints = shieldPoints;

            Shield = shield;
        }

        public Player Player { get; }

        public ulong ShieldPoints { get; }

        public byte Shield { get; }
    }
}