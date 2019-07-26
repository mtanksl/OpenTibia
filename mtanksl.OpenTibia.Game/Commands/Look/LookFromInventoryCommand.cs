using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromInventoryCommand : Command
    {
        public LookFromInventoryCommand(Player player, byte fromSlot, ushort itemId)
        {
            Player = player;

            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item item = fromInventory.GetContent(FromSlot) as Item;

            if (item != null && item.Metadata.TibiaId == ItemId)
            {
                LookItemCommand command = new LookItemCommand(Player, item);

                command.Completed += (s, e) =>
                {
                    //Act

                    base.Execute(server, context);
                };

                command.Execute(server, context);
            }
        }
    }
}