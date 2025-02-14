using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateOutfitCommand : Command
    {
        public CreatureUpdateOutfitCommand(Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool conditionStealth)
        {
            Creature = creature;

            BaseOutfit = baseOutfit;

            ConditionOutfit = conditionOutfit;

            Swimming = swimming;

            ConditionStealth = conditionStealth;
        }

        public Creature Creature { get; set; }

        public Outfit BaseOutfit { get; set; }

        public Outfit ConditionOutfit { get; set; }

        public bool Swimming { get; set; }

        public bool ConditionStealth { get; set; }

        public override Promise Execute()
        {
            if (Creature.BaseOutfit != BaseOutfit || Creature.ConditionOutfit != ConditionOutfit || Creature.Swimming != Swimming || Creature.ConditionStealth != ConditionStealth)
            {
                Creature.BaseOutfit = BaseOutfit;
                Creature.ConditionOutfit = ConditionOutfit;
                Creature.Swimming = Swimming;
                Creature.ConditionStealth = ConditionStealth;

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

                Context.AddEvent(new CreatureUpdateOutfitEventArgs(Creature, BaseOutfit, ConditionOutfit, Swimming, ConditionStealth) );
            }

            return Promise.Completed;
        }
    }
}