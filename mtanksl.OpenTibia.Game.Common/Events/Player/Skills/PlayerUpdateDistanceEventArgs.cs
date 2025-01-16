using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateDistanceEventArgs : GameEventArgs
    {
        public PlayerUpdateDistanceEventArgs(Player player, ulong distancePoints, byte distance)
        {
            Player = player;

            DistancePoints = distancePoints;

            Distance = distance;
        }

        public Player Player { get; }

        public ulong DistancePoints { get; }

        public byte Distance { get; }
    }
}