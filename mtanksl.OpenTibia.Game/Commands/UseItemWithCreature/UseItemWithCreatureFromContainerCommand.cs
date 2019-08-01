using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromContainerCommand : UseItemWithCreatureCommand
    {
        public UseItemWithCreatureFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, uint toCreatureId) : base(player)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = server.Map.GetCreature(ToCreatureId);

                    if (toCreature != null)
                    {
                        //Act

                        if ( IsUseable(fromItem, server, context) )
                        {
                            UseItemWithCreature(fromItem, toCreature, server, context, () =>
                            {
                                switch (fromContainer.GetRootContainer() )
                                {
                                    case Tile fromTile:

                                        MoveItemFromContainerToInventoryCommand moveItemFromTileToInventoryCommand = new MoveItemFromContainerToInventoryCommand(Player, FromContainerId, FromContainerIndex, ItemId, (byte)Slot.Extra, 1);

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

                                        break;

                                    case Inventory fromInventory:

                                        WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, toCreature.Tile);

                                        walkToUnknownPathCommand.Completed += (s, e) =>
                                        {
                                            server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), Constants.PlayerSchedulerEventDelay, this);
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