using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromContainerToTileCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId) :base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            FromItemId = fromItemId;

            ToPosition = toPosition;

            ToIndex = ToIndex;

            ToItemId = toItemId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort FromItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        Item toItem = toTile.GetContent(ToIndex) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                        {
                            //Act

                            if ( IsUseable(fromItem, server, context) )
                            {
                                UseItemWithItem(fromItem, toItem, toTile, server, context, () =>
                                {
                                    switch (fromContainer.GetRootContainer() )
                                    {
                                        case Tile fromTile:

                                            MoveItemFromContainerToInventoryCommand moveItemFromTileToInventoryCommand = new MoveItemFromContainerToInventoryCommand(Player, FromContainerId, FromContainerIndex, FromItemId, (byte)Slot.Extra, 1);

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

                                            break;

                                        case Inventory fromInventory:

                                            WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, toTile);

                                            walkToUnknownPathCommand.Completed += (s, e) =>
                                            {
                                                server.QueueForExecution(Constants.PlayerActionSchedulerEvent(Player), Constants.PlayerSchedulerEventDelay, this);
                                            };

                                            walkToUnknownPathCommand.Execute(server, context);

                                            break;
                                    }
                                } );
                            }
                        }
                    }
                }
            }
        }
    }
}