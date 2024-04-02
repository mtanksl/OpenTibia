using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateOutfitCommand : Command
    {
        public CreatureUpdateOutfitCommand(Creature creature, Outfit baseOutfit, Outfit outfit)
        {
            Creature = creature;

            BaseOutfit = baseOutfit;

            Outfit = outfit;
        }

        public Creature Creature { get; set; }

        public Outfit BaseOutfit { get; set; }

        public Outfit Outfit { get; set; }

        public override Promise Execute()
        {
            if (Creature.BaseOutfit != BaseOutfit || Creature.Outfit != Outfit)
            {
                Creature.BaseOutfit = BaseOutfit;

                Creature.Outfit = Outfit;

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

                Context.AddEvent(new CreatureUpdateOutfitEventArgs(Creature, BaseOutfit, Outfit) );
            }

            return Promise.Completed;
        }
    }
}