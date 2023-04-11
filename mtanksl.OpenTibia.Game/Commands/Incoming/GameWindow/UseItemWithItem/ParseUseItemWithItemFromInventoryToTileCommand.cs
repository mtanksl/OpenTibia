using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromInventoryToTileCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromInventoryToTileCommand(Player player, byte fromSlot, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId) : base(player)
        {
            FromSlot = fromSlot;

            FromItemId = fromItemId;

            ToPosition = toPosition;

            ToIndex = toIndex;

            ToItemId = toItemId;
        }

        public byte FromSlot { get; set; }

        public ushort FromItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Inventory fromInventory = Player.Inventory;

                Item fromItem = fromInventory.GetContent(FromSlot) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Tile toTile = Context.Server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        switch (toTile.GetContent(ToIndex) )
                        {
                            case Item toItem:

                                if (toItem.Metadata.TibiaId == ToItemId)
                                {
                                    if ( IsUseable(Context, fromItem) )
                                    {
                                        Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) ).Then( () =>
                                        {
                                            resolve();
                                        } );
                                    }
                                }

                                break;

                            case Creature toCreature:

                                if (ToItemId == 99)
                                {
                                    if ( IsUseable(Context, fromItem) )
                                    {
                                        Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) ).Then( () =>
                                        {
                                            resolve();
                                        } );
                                    }
                                }

                                break;
                        }
                    }
                }
            } );
        }
    }
}