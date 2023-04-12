using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateOutfit : Command
    {
        public CreatureUpdateOutfit(Creature creature, Outfit outfit)
        {
            Creature = creature;

            Outfit = outfit;
        }

        public Creature Creature { get; set; }

        public Outfit Outfit { get; set; }

        public override Promise Execute()
        {
            if (Creature.Outfit != Outfit)
            {
                Creature.Outfit = Outfit;

                Tile fromTile = Creature.Tile;

                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        Context.AddPacket(observer.Client.Connection, new SetOutfitOutgoingPacket(Creature.Id, Creature.Outfit) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}