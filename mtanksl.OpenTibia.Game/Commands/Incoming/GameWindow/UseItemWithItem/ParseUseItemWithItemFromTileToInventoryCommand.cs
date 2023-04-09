using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromTileToInventoryCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromTileToInventoryCommand(Player player, Position fromPosition, byte fromIndex, ushort fromItemId, byte toSlot, ushort toItemId) :base(player)
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

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Tile fromTile = context.Server.Map.GetTile(FromPosition);

                if (fromTile != null)
                {
                    if (Player.Tile.Position.CanSee(fromTile.Position) )
                    {
                        Item fromItem = fromTile.GetContent(FromIndex) as Item;

                        if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                        {
                            Inventory toInventory = Player.Inventory;

                            Item toItem = toInventory.GetContent(ToSlot) as Item;

                            if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                            {
                                if ( IsUseable(context, fromItem) )
                                {
                                    context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) ).Then(ctx =>
                                    {
                                        resolve(context);
                                    } );
                                }
                            }
                        }
                    }
                }
            } );
        }
    }
}