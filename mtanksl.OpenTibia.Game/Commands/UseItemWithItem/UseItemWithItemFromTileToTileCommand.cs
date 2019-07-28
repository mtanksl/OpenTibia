using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromTileToTileCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromTileToTileCommand(Player player, Position fromPosition, byte fromIndex, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            FromItemId = fromItemId;

            ToPosition = toPosition;

            ToIndex = ToIndex;

            ToItemId = toItemId;
        }
        
        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort FromItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        Item toItem = toTile.GetContent(ToIndex) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                        {
                            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
                            {
                                //Act

                                if ( IsNextTo(fromTile, server, context) )
                                {
                                    UseItemWithItem(fromItem, toItem, toTile, server, context, () =>
                                    {
                                        MoveItemFromTileToInventoryCommand moveItemFromTileToInventoryCommand = new MoveItemFromTileToInventoryCommand(Player, FromPosition, FromIndex, FromItemId, (byte)Slot.Extra, 1);

                                        moveItemFromTileToInventoryCommand.Completed += (s, e) =>
                                        {
                                            UseItemWithItemFromInventoryToTileCommand useItemWithItemFromInventoryToTileCommand = new UseItemWithItemFromInventoryToTileCommand(Player, (byte)Slot.Extra, FromItemId, ToPosition, ToIndex, ToItemId);

                                            useItemWithItemFromInventoryToTileCommand.Completed += (s2, e2) =>
                                            {
                                                base.Execute(e2.Server, e2.Context);
                                            };

                                            useItemWithItemFromInventoryToTileCommand.Execute(e.Server, e.Context);
                                        };

                                        moveItemFromTileToInventoryCommand.Execute(server, context);
                                    } );
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}