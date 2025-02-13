using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateOutfitCommand : Command
    {
        public CreatureUpdateOutfitCommand(Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool stealth)
        {
            Creature = creature;

            BaseOutfit = baseOutfit;

            ConditionOutfit = conditionOutfit;

            Swimming = swimming;

            Stealth = stealth;
        }

        public Creature Creature { get; set; }

        public Outfit BaseOutfit { get; set; }

        public Outfit ConditionOutfit { get; set; }

        public bool Swimming { get; set; }

        public bool Stealth { get; set; }

        public override Promise Execute()
        {
            if (Creature.BaseOutfit != BaseOutfit || Creature.ConditionOutfit != ConditionOutfit || Creature.Swimming != Swimming || Creature.Stealth != Stealth)
            {
                Creature.BaseOutfit = BaseOutfit;
                Creature.ConditionOutfit = ConditionOutfit;
                Creature.Swimming = Swimming;
                Creature.Stealth = Stealth;

                if (Creature.Tile != null)
                {
                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                    {
                        byte clientIndex;

                        if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            Context.AddPacket(observer, new SetOutfitOutgoingPacket(Creature.Id, Creature.Outfit) );
                        }
                    }
                }

                Context.AddEvent(new CreatureUpdateOutfitEventArgs(Creature, BaseOutfit, ConditionOutfit, Swimming, Stealth) );
            }

            return Promise.Completed;
        }
    }
}