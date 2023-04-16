using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerDeathEventArgs : GameEventArgs
    {
        public PlayerDeathEventArgs(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
    }
}