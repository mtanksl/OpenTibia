using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureWalkEventArgs : GameEventArgs
    {
        public CreatureWalkEventArgs(Creature creature, Tile fromTile, byte fromIndex, Tile toTile, byte toIndex)
        {
            Creature = creature;

            FromTile = fromTile;

            FromIndex = fromIndex;

            ToTile = toTile;

            ToIndex = toIndex;
        }

        public Creature Creature { get; set; }

        public Tile FromTile { get; set; }

        public byte FromIndex { get; set; }   

        public Tile ToTile { get; set; }

        public byte ToIndex { get; set; }
    }
}