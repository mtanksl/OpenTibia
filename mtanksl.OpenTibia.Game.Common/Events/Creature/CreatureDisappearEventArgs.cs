using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureDisappearEventArgs : GameEventArgs
    {
        public CreatureDisappearEventArgs(Creature creature, Tile fromTile, int? fromIndex)
        {
            Creature = creature;

            FromTile = fromTile;

            FromIndex = fromIndex;
        }

        public Creature Creature { get; }

        public Tile FromTile { get; }

        public int? FromIndex { get; }
    }
}