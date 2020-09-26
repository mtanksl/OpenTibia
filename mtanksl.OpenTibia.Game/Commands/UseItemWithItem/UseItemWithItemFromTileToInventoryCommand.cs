using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromTileToInventoryCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromTileToInventoryCommand(Player player, Position fromPosition, byte fromIndex, ushort fromItemId, byte toSlot, ushort toItemId) :base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            FromItemId = fromItemId;

            ToSlot = toSlot;

            ToItemId = toItemId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort FromItemId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Inventory toInventory = Player.Inventory;

                    Item toItem = toInventory.GetContent(ToSlot) as Item;

                    if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                    {
                        if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
                        {
                            //Act

                            if ( IsUseable(fromItem, context) &&
                                
                                IsNextTo(fromTile, context) )
                            {
                                UseItemWithItem(fromItem, toItem, context);
                            }
                        }
                    }
                }
            }
        }
    }
}