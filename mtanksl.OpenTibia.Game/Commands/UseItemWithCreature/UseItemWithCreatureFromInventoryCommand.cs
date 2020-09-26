using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromInventoryCommand : UseItemWithCreatureCommand
    {
        public UseItemWithCreatureFromInventoryCommand(Player player, byte fromSlot, ushort itemId, uint toCreatureId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Creature toCreature = context.Server.Map.GetCreature(ToCreatureId);

                if (toCreature != null)
                {
                    //Act

                    if ( IsUseable(fromItem, context) )
                    {
                        UseItemWithCreature(fromItem, toCreature, context, () =>
                        {
                            WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, toCreature.Tile);

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