using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureAppearEventArgs : GameEventArgs
    {
        public CreatureAppearEventArgs(Creature creature, Tile toTile)
        {
            Creature = creature;

            ToTile = toTile;
        }

        public Creature Creature { get; }

        public Tile ToTile { get; }
    }
}