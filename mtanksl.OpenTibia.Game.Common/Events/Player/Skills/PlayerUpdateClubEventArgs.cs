using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateClubEventArgs : GameEventArgs
    {
        public PlayerUpdateClubEventArgs(Player player, ulong clubPoints, byte club)
        {
            Player = player;

            ClubPoints = clubPoints;

            Club = club;
        }

        public Player Player { get; }

        public ulong ClubPoints { get; }

        public byte Club { get; }
    }
}