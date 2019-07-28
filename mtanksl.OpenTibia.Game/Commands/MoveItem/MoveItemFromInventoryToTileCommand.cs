using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToTileCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToTileCommand(Player player, byte fromSlot, ushort itemId, Position toPosition, byte count) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Tile toTile = server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    //Act

                    if ( IsMoveable(fromItem, server, context) &&

                        CanThrow(Player.Tile, toTile, server, context) )
                    {
                        Tile nextTile = server.Map.GetNextTile(toTile);

                        if (nextTile == null)
                        {
                            nextTile = toTile;
                        }

                        MoveItem(fromItem, nextTile, 0, Count, server, context, () =>
                        {
                            new InventoryRemoveItemCommand(fromInventory, FromSlot).Execute(server, context);

                            new TileAddItemCommand(nextTile, fromItem).Execute(server, context);
                        } );
                    }
                }
            }
        }
    }
}