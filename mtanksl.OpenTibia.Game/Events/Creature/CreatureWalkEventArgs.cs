using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureWalkEventArgs : GameEventArgs
    {
        public CreatureWalkEventArgs(Creature creature, Tile fromTile, int fromIndex, Tile toTile, int toIndex)
        {
            Creature = creature;

            FromTile = fromTile;

            FromIndex = fromIndex;

            ToTile = toTile;

            ToIndex = toIndex;
        }

        public Creature Creature { get; set; }

        public Tile FromTile { get; set; }

        public int FromIndex { get; set; }   

        public Tile ToTile { get; set; }

        public int ToIndex { get; set; }
    }
}