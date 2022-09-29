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

        public override void Execute(Context context)
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Tile toTile = context.Server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    if (IsMoveable(context, fromItem, Count) && CanThrow(context, Player.Tile, toTile))
                    {
                        MoveItem(context, fromItem, toTile, 0, Count);
                    }
                }
            }
        }
    }
}