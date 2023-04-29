using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

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

                foreach (var observer in Context.Server.Map.GetObservers(Creature.Tile.Position).OfType<Player>() )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer.Client.Connection, new SetOutfitOutgoingPacket(Creature.Id, Creature.Outfit) );
                    }
                }

                Context.AddEvent(new CreatureUpdateOutfitEventArgs(Creature, BaseOutfit, Outfit) );
            }

            return Promise.Completed;
        }
    }
}