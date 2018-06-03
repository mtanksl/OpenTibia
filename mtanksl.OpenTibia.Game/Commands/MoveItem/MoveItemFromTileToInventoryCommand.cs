using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToInventoryCommand : Command
    {
        private Server server;

        public MoveItemFromTileToInventoryCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public byte ToSlot { get; set; }
        
        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            Item fromItem = (Item)fromTile.GetContent(FromIndex);

            Inventory toInventory = Player.Inventory;

            Item toItem = (Item)toInventory.GetContent(ToSlot);

            //Act

            if (toItem == null)
            {
                fromTile.RemoveContent(fromItem);

                toInventory.AddContent(ToSlot, fromItem);

                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                         context.Response.Write(observer.Client.Connection, new ThingRemove(fromTile.Position, FromIndex) );
                    }
                }

                context.Response.Write(Player.Client.Connection, new SlotAdd( (Slot)ToSlot, fromItem) );
            }
        }
    }
}