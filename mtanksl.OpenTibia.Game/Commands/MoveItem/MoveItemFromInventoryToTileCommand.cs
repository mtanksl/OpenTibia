using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToTileCommand : Command
    {
        public MoveItemFromInventoryToTileCommand(Player player, byte fromSlot, Position toPosition)
        {
            Player = player;

            FromSlot = fromSlot;

            ToPosition = toPosition;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public Position ToPosition { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = (Item)fromInventory.GetContent(FromSlot);

            Tile toTile = server.Map.GetTile(ToPosition);

            //Act

            fromInventory.RemoveContent(fromItem);

            byte toIndex = toTile.AddContent(fromItem);

            //Notify

            context.Write(Player.Client.Connection, new SlotRemove( (Slot)FromSlot) );

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(toTile.Position) )
                {
                     context.Write(observer.Client.Connection, new ThingAdd(toTile.Position, toIndex, fromItem) );
                }
            }
        }
    }
}