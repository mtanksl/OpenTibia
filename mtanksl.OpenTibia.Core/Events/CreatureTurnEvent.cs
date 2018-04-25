namespace OpenTibia
{
    public class CreatureTurnEvent : GameEvent
    {
        public CreatureTurnEvent(Creature creature, Direction fromDirection, Direction toDirection)
        {
            Creature = creature;

            FromDirection = fromDirection;

            ToDirection = toDirection;
        }

        public Creature Creature { get; set; }

        public Direction FromDirection { get; set; }

        public Direction ToDirection { get; set; }
    }
}