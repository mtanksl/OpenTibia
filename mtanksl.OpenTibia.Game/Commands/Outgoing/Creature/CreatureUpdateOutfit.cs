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

        public override void Execute(Context context)
        {
            if (Creature.Outfit != Outfit)
            {
                Tile fromTile = Creature.Tile;

                Creature.Outfit = Outfit;

                foreach (var observer in context.Server.GameObjects.GetPlayers())
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position))
                    {
                        context.AddPacket(observer.Client.Connection, new SetOutfitOutgoingPacket(Creature.Id, Creature.Outfit));
                    }
                }
            }

            base.Execute(context);
        }
    }
}