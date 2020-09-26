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

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = context.Server.Map.GetCreature(ToCreatureId);

                    if (toCreature != null)
                    {
                        //Act

                        if ( IsUseable(fromItem, context) &&

                            IsNextTo(fromTile, context) )
                        {
                            UseItemWithCreature(fromItem, toCreature, context, () =>
                            {
                                MoveItemFromTileToInventoryCommand moveItemFromTileToInventoryCommand = new MoveItemFromTileToInventoryCommand(Player, FromPosition, FromIndex, ItemId, (byte)Slot.Extra, 1);

                                moveItemFromTileToInventoryCommand.Completed += (s, e) =>
                                {
                                    UseItemWithCreatureFromInventoryCommand useItemWithCreatureFromInventoryCommand = new UseItemWithCreatureFromInventoryCommand(Player, (byte)Slot.Extra, ItemId, ToCreatureId);

                                    useItemWithCreatureFromInventoryCommand.Completed += (s2, e2) =>
                                    {
                                        base.Execute(e2.Context);
                                    };

                                    useItemWithCreatureFromInventoryCommand.Execute(e.Context);
                                };

                                moveItemFromTileToInventoryCommand.Execute(context);
                            } );
                        }
                    }
                }
            }
        }
    }
}