using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class SelectedOutfitCommand : Command
    {
        public SelectedOutfitCommand(Creature creature, Outfit outfit)
        {
            Creature = creature;

            Outfit = outfit;
        }

        public Creature Creature { get; set; }

        public Outfit Outfit { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Creature.Tile;

            byte fromIndex = fromTile.GetIndex(Creature);

            if (Creature.Outfit != Outfit)
            {
                //Act

                Creature.Outfit = Outfit;

                //Notify

                foreach (var observer in context.Server.Map.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new SetOutfitOutgoingPacket(Creature.Id, Outfit) );
                    }
                }
            }

            base.Execute(context);
        }
    }
}