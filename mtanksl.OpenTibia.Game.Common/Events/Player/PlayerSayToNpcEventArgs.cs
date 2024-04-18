using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerSayToNpcEventArgs : GameEventArgs
    {
        public PlayerSayToNpcEventArgs(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; }

        public string Message { get; }
    }
}