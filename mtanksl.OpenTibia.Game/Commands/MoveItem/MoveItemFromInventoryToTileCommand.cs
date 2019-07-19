using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToTileCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToTileCommand(Player player, byte fromSlot, Position toPosition, byte count)
        {
            Player = player;

            FromSlot = fromSlot;

            ToPosition = toPosition;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null)
            {
                Tile toTile = server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    //Act

                    RemoveItem(fromInventory, fromItem, server, context);

                    AddItem(toTile, fromItem, server, context);
                }
            }
        }
    }
}