namespace OpenTibia
{
    public class CreatureChangeHealthEvent : GameEvent
    {
        public CreatureChangeHealthEvent(Creature creature, ushort fromHealth, ushort toHealth)
        {
            Creature = creature;

            FromHealth = fromHealth;

            ToHealth = toHealth;
        }

        public Creature Creature { get; set; }

        public ushort FromHealth { get; set; }

        public ushort ToHealth { get; set; }
    }
}