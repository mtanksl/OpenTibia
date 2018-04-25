namespace OpenTibia
{
    public class CreatureChangeOutfitEvent : GameEvent
    {
        public CreatureChangeOutfitEvent(Creature creature, Outfit fromOutfit, Outfit toOutfit)
        {
            Creature = creature;

            FromOutfit = fromOutfit;

            ToOutfit = toOutfit;
        }

        public Creature Creature { get; set; }

        public Outfit FromOutfit { get; set; }

        public Outfit ToOutfit { get; set; }
    }
}