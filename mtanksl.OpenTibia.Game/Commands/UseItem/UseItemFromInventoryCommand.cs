using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromInventoryCommand : UseItemCommand
    {
        public UseItemFromInventoryCommand(Player player, byte fromSlot, ushort itemId)
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

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                //Act

                Container container = fromItem as Container;

                if (container != null)
                {
                    CloseOrOpenContainer(Player, container, server, context);
                }

                base.Execute(server, context);
            }
        }
    }
}