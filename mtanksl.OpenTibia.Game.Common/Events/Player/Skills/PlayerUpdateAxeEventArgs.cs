using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateAxeEventArgs : GameEventArgs
    {
        public PlayerUpdateAxeEventArgs(Player player, ulong axePoints, byte axe)
        {
            Player = player;

            AxePoints = axePoints;

            Axe = axe;
        }

        public Player Player { get; }

        public ulong AxePoints { get; }

        public byte Axe { get; }
    }
}