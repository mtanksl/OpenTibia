using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateSoulEventArgs : GameEventArgs
    {
        public PlayerUpdateSoulEventArgs(Player player, byte soul)
        {
            Player = player;

            Soul = soul;
        }

        public Player Player { get; }

        public byte Soul { get; }
    }
}