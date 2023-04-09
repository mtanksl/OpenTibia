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
                        Tile toTile = context.Server.Map.GetTile(ToPosition);

                        if (toTile != null)
                        {
                            switch (toTile.GetContent(ToIndex) )
                            {
                                case Item toItem:

                                    if (toItem.Metadata.TibiaId == ToItemId)
                                    {
                                        if ( IsUseable(context, fromItem) )
                                        {
                                            context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) ).Then(ctx =>
                                            {
                                                resolve(context);
                                            } );
                                        }
                                    }

                                    break;

                                case Creature toCreature:

                                    if (ToItemId == 99)
                                    {
                                        if ( IsUseable(context, fromItem) )
                                        {
                                            context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) ).Then(ctx =>
                                            {
                                                resolve(context);
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