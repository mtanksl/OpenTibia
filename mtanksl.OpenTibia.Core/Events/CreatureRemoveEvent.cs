namespace OpenTibia
{
    public class CreatureRemoveEvent : GameEvent
    {
        public CreatureRemoveEvent(Creature creature, Tile fromTile, byte fromIndex)
        {
            Creature = creature;

            FromTile = fromTile;

            FromIndex = fromIndex;
        }

        public Creature Creature { get; set; }

        public Tile FromTile { get; set; }

        public byte FromIndex { get; set; }
    }
}