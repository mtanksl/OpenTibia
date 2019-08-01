using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileReplaceItemCommand : Command
    {
        public TileReplaceItemCommand(Tile tile, Item fromItem, Item toItem)
        {
            Tile = tile;

            FromItem = fromItem;

            ToItem = toItem;
        }

        public Tile Tile { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }
        
        public override void Execute(Server server, Context context)
        {
            //Arrange

            byte index = Tile.GetIndex(FromItem);

            //Act

            Tile.ReplaceContent(index, ToItem);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, index, ToItem) );
                }
            }

            //Event

            if (server.Events.TileRemoveItem != null)
            {
                server.Events.TileRemoveItem(this, new TileRemoveItemEventArgs(FromItem, Tile, index, server, context) );
            }

            if (server.Events.TileAddItem != null)
            {
                server.Events.TileAddItem(this, new TileAddItemEventArgs(ToItem, Tile, index, server, context) );
            }

            base.Execute(server, context);
        }
    }
}