using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureDisappearEventArgs : GameEventArgs
    {
        public CreatureDisappearEventArgs(Creature creature, Tile fromTile)
        {
            Creature = creature;

            FromTile = fromTile;
        }

        public Creature Creature { get; }

        public Tile FromTile { get; }
    }
}