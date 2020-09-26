using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileAddItemCommand : Command
    {
        public TileAddItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            byte index = Tile.AddContent(Item);

            //Notify

            foreach (var observer in context.Server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, Item) );
                }
            }

            //Event

            if (context.Server.Events.TileAddItem != null)
            {
                context.Server.Events.TileAddItem(this, new TileAddItemEventArgs(Tile, Item, index) );
            }

            base.Execute(context);
        }
    }
}