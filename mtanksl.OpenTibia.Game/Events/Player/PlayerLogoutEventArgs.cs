using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerLogoutEventArgs : GameEventArgs
    {
        public PlayerLogoutEventArgs(Tile tile, Player player)
        {
            Tile = tile;

            Player = player;
        }

        public Tile Tile { get; set; }

        public Player Player { get; set; }
    }
}