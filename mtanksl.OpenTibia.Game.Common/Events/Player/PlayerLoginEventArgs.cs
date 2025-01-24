using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerLoginEventArgs : GameEventArgs
    {
        public PlayerLoginEventArgs(Player player)
        {
            Player = player;
        }

        public Player Player { get; }
    }
}