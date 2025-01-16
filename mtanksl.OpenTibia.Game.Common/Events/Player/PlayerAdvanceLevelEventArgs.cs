using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerAdvanceLevelEventArgs : GameEventArgs
    {
        public PlayerAdvanceLevelEventArgs(Player player, ushort fromLevel, ushort toLevel)
        {
            Player = player;

            FromLevel = fromLevel;

            ToLevel = toLevel;
        }

        public Player Player { get; }

        public ushort FromLevel { get; }

        public ushort ToLevel { get; }
    }
}