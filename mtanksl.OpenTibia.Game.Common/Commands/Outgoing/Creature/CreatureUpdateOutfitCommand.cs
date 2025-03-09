using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateOutfitCommand : Command
    {
        public CreatureUpdateOutfitCommand(Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool conditionStealth, bool itemStealth, bool isMounted)
        {
            Creature = creature;

            BaseOutfit = baseOutfit;

            ConditionOutfit = conditionOutfit;

            Swimming = swimming;

            ConditionStealth = conditionStealth;

            ItemStealth = itemStealth;

            IsMounted = isMounted;
        }

        public Creature Creature { get; set; }

        public Outfit BaseOutfit { get; set; }

        public Outfit ConditionOutfit { get; set; }

        public bool Swimming { get; set; }

        public bool ConditionStealth { get; set; }

        public bool ItemStealth { get; set; }

        public bool IsMounted { get; set; }

        public override Promise Execute()
        {
            if (Creature.BaseOutfit != BaseOutfit || Creature.ConditionOutfit != ConditionOutfit || Creature.Swimming != Swimming || Creature.ConditionStealth != ConditionStealth || Creature.ItemStealth != ItemStealth || Creature.IsMounted != IsMounted)
            {
                Creature.BaseOutfit = BaseOutfit;
                Creature.ConditionOutfit = ConditionOutfit;
                Creature.Swimming = Swimming;
                Creature.ConditionStealth = ConditionStealth;
                Creature.ItemStealth = ItemStealth;
                Creature.IsMounted = IsMounted;

                if (Creature.Tile != null)
                {
                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                    {
                        byte clientIndex;

                        if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            Context.AddPacket(observer, new SetOutfitOutgoingPacket(Creature.Id, Creature.ClientOutfit) );
                        }
                    }
                }

                Context.AddEvent(new CreatureUpdateOutfitEventArgs(Creature, BaseOutfit, ConditionOutfit, Swimming, ConditionStealth, ItemStealth) );
            }

            return Promise.Completed;
        }
    }
}