using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromTileCommand : UseItemWithCreatureCommand
    {
        public UseItemWithCreatureFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, uint toCreatureId) : base(player)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = server.Map.GetCreature(ToCreatureId);

                    if (toCreature != null)
                    {
                        if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
                        {
                            //Act

                            if ( IsNextTo(fromTile, server, context) )
                            {
                                UseItemWithCreature(fromItem, toCreature, server, context, () =>
                                {
                                    MoveItemFromTileToInventoryCommand moveItemFromTileToInventoryCommand = new MoveItemFromTileToInventoryCommand(Player, FromPosition, FromIndex, ItemId, (byte)Slot.Extra, 1);

                                    moveItemFromTileToInventoryCommand.Completed += (s, e) =>
                                    {
                                        UseItemWithCreatureFromInventoryCommand useItemWithCreatureFromInventoryCommand = new UseItemWithCreatureFromInventoryCommand(Player, (byte)Slot.Extra, ItemId, ToCreatureId);

                                        useItemWithCreatureFromInventoryCommand.Completed += (s2, e2) =>
                                        {
                                            base.Execute(e2.Server, e2.Context);
                                        };

                                        useItemWithCreatureFromInventoryCommand.Execute(e.Server, e.Context);
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