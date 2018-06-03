using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToTileCommand : Command
    {
        private Server server;

        public MoveItemFromInventoryToTileCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public Position ToPosition { get; set; }
        
        public override void Execute(Context context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = (Item)fromInventory.GetContent(FromSlot);

            Tile toTile = server.Map.GetTile(ToPosition);

            //Act

            fromInventory.RemoveContent(fromItem);

            byte toIndex = toTile.AddContent(fromItem);

            //Notify

            context.Response.Write(Player.Client.Connection, new SlotRemove( (Slot)FromSlot) );

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(toTile.Position) )
                {
                     context.Response.Write(observer.Client.Connection, new ThingAdd(toTile.Position, toIndex, fromItem) );
                }
            }
        }
    }
}