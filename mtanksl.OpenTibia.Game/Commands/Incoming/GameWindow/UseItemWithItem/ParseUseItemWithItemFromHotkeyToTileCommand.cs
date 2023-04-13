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
                                    if ( IsUseable(fromItem) )
                                    {
                                        return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                                    }
                                }

                                break;

                            case Creature toCreature:

                                if (ToItemId == 99)
                                {
                                    if ( IsUseable(fromItem) )
                                    {
                                        return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
                                    }
                                }

                                break;
                        }
                    }

                    break;
                }
            }

            return Promise.Break;
        }
    }
}