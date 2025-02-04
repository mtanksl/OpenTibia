using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureAppearEventArgs : GameEventArgs
    {
        public CreatureAppearEventArgs(Creature creature, Tile toTile, int? toIndex)
        {
            Creature = creature;

            ToTile = toTile;

            ToIndex = toIndex;
        }

        public Creature Creature { get; }

        public Tile ToTile { get; }

        public int? ToIndex { get; }
    }
}