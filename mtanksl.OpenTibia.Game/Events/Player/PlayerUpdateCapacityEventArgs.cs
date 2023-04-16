using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateCapacityEventArgs : GameEventArgs
    {
        public PlayerUpdateCapacityEventArgs(Player player, uint capacity)
        {
            Player = player;

            Capacity = capacity;
        }

        public Player Player { get; set; }

        public uint Capacity { get; set; }
    }
}