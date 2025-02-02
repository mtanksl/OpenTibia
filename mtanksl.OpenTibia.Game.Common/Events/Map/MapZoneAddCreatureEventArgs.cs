using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class MapZoneAddCreatureEventArgs : GameEventArgs
    {
        public MapZoneAddCreatureEventArgs(Creature creature, Tile fromTile, int? fromIndex, Tile toTile, int? toIndex)
        {
            Creature = creature;

            FromTile = fromTile;

            FromIndex = fromIndex;

            ToTile = toTile;

            ToIndex = toIndex;
        }

        public Creature Creature { get; }

        public Tile FromTile { get; }

        public int? FromIndex { get; }

        public Tile ToTile { get; }

        public int? ToIndex { get; }
    }
}