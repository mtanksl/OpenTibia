using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromTileToInventoryCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromTileToInventoryCommand(Player player, Position fromPosition, byte fromIndex, ushort fromTibiaId, byte toSlot, ushort toTibiaId) :base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            FromTibiaId = fromTibiaId;

            ToSlot = toSlot;

            ToTibiaId = toTibiaId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort FromTibiaId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Tile fromTile = Context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                if (Player.Tile.Position.CanHearSay(fromTile.Position) )
                {
                    Item fromItem = Player.Client.GetContent(fromTile, FromIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == FromTibiaId)
                    {
                        Inventory toInventory = Player.Inventory;

                        Item toItem = toInventory.GetContent(ToSlot) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToTibiaId)
                        {
                            if ( IsUseable(fromItem) )
                            {
                                if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
                                {
                                    return Context.AddCommand(new CreatureWalkToCommand(Player, fromTile) ).Then( () =>
                                    {
                                        return Execute();
                                    } );
                                }

                                return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                            }
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}