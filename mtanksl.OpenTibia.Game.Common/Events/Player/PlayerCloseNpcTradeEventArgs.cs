using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerCloseNpcTradeEventArgs : GameEventArgs
    {
        public PlayerCloseNpcTradeEventArgs(Player player)
        {
            Player = player;
        }

        public Player Player { get; }
    }
}