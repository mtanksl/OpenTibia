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

        public override void Execute(Context context)
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = context.Server.GameObjects.GetGameObject<Creature>(ToCreatureId);

                    if (toCreature != null)
                    {
                        if ( IsUseable(fromItem, context) )
                        {
                            UseItemWithCreature(fromItem, toCreature, context, () =>
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
                                                base.OnCompleted(e2.Context);
                                            };

                                            useItemWithCreatureFromInventoryCommand.Execute(e.Context);
                                        };

                                        moveItemFromTileToInventoryCommand.Execute(context);

                                        break;

                                    case Inventory fromInventory:

                                        WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, toCreature.Tile);

                                        walkToUnknownPathCommand.Completed += (s, e) =>
                                        {
                                            context.Server.QueueForExecution(Constants.CreatureActionSchedulerEvent(Player), Constants.CreatureActionSchedulerEventDelay, this);
                                        };

                                        walkToUnknownPathCommand.Execute(context);

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