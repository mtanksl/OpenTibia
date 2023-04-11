using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromHotkeyToTileCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromHotkeyToTileCommand(Player player, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId) : base(player)
        {
            FromItemId = fromItemId;

            ToPosition = toPosition;

            ToIndex = toIndex;

            ToItemId = toItemId;
        }

        public ushort FromItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Inventory fromInventory = Player.Inventory;

                foreach (var pair in fromInventory.GetIndexedContents() )
                {
                    Item fromItem = (Item)pair.Value;

                    if (fromItem.Metadata.TibiaId == FromItemId)
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

                        break;
                    }
                }
            } );
        }
    }
}