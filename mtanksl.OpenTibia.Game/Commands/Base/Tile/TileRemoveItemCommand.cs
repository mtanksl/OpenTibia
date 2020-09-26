using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileRemoveItemCommand : Command
    {
        public TileRemoveItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            byte index = Tile.GetIndex(Item);

            //Act

            Tile.RemoveContent(index);

            //Notify

            foreach (var observer in context.Server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, index) );
                }
            }

            //Event

            if (context.Server.Events.TileRemoveItem != null)
            {
                context.Server.Events.TileRemoveItem(this, new TileRemoveItemEventArgs(Tile, Item, index) );
            }

            base.Execute(context);
        }
    }
}