using OpenTibia.Common.Objects;
using OpenTibia.Game;

namespace OpenTibia.Common.Events
{
    public class LogoutEventArgs : GameEventArgs
    {
        public LogoutEventArgs(Player player, Tile tile, byte index, Server server, Context context) : base(server, context)
        {
            Player = player;

            Tile = tile;

            Index = index;
        }

        public Player Player { get; set; }

        public Tile Tile { get; set; }

        public byte Index { get; set; }
    }
}