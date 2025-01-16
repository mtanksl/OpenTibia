using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateFistEventArgs : GameEventArgs
    {
        public PlayerUpdateFistEventArgs(Player player, ulong fistPoints, byte fist)
        {
            Player = player;

            FistPoints = fistPoints;

            Fist = fist;
        }

        public Player Player { get; }

        public ulong FistPoints { get; }

        public byte Fist { get; }
    }
}