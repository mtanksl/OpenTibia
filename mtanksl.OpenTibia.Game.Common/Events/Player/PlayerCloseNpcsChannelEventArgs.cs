using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerCloseNpcsChannelEventArgs : GameEventArgs
    {
        public PlayerCloseNpcsChannelEventArgs(Player player)
        {
            Player = player;
        }

        public Player Player { get; }
    }
}