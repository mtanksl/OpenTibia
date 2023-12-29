using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerLogoutEventArgs : GameEventArgs
    {
        public PlayerLogoutEventArgs(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
    }
}