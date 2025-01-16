using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerUpdateFishEventArgs : GameEventArgs
    {
        public PlayerUpdateFishEventArgs(Player player, ulong fishPoints, byte fish)
        {
            Player = player;

            FishPoints = fishPoints;

            Fish = fish;
        }

        public Player Player { get; }

        public ulong FishPoints { get; }

        public byte Fish { get; }
    }
}