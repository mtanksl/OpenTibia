using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromInventoryToTileCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromInventoryToTileCommand(Player player, byte fromSlot, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId) : base(player)
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

        public override void Execute(Context context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
            {
                Tile toTile = context.Server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    Item toItem = toTile.GetContent(ToIndex) as Item;

                    if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                    {
                        //Act

                        if ( IsUseable(fromItem, context) )
                        {
                            UseItemWithItem(fromItem, toItem, toTile, context, () =>
                            {
                                WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, toTile);

                                walkToUnknownPathCommand.Completed += (s, e) =>
                                {
                                    context.Server.QueueForExecution(Constants.CreatureActionSchedulerEvent(Player), Constants.CreatureActionSchedulerEventDelay, this);
                                };

                                walkToUnknownPathCommand.Execute(context);
                            } );
                        }
                    }
                }
            }
        }
    }
}