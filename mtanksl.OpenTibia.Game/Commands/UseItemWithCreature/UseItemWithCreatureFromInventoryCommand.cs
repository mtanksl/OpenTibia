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

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

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
                            WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, toCreature.Tile);

                            walkToUnknownPathCommand.Completed += (s, e) =>
                            {
                                server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), Constants.PlayerSchedulerEventDelay, this);
                            };

                            walkToUnknownPathCommand.Execute(server, context);
                        } );
                    }
                }
            }
        }
    }
}