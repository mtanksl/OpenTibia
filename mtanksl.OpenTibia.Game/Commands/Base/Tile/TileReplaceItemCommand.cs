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
        
        public override void Execute(Context context)
        {
            //Arrange

            byte index = Tile.GetIndex(FromItem);

            //Act

            Tile.ReplaceContent(index, ToItem);

            //Notify

            foreach (var observer in context.Server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, index, ToItem) );
                }
            }

            //Event

            if (context.Server.Events.TileRemoveItem != null)
            {
                context.Server.Events.TileRemoveItem(this, new TileRemoveItemEventArgs(Tile, FromItem, index) );
            }

            if (context.Server.Events.TileAddItem != null)
            {
                context.Server.Events.TileAddItem(this, new TileAddItemEventArgs(Tile, ToItem, index) );
            }

            base.Execute(context);
        }
    }
}