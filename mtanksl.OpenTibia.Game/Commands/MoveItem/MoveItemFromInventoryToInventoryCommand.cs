using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToInventoryCommand(Player player, byte fromSlot, byte toSlot, byte count)
        {
            Player = player;

            FromSlot = fromSlot;

            ToSlot = toSlot;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public byte ToSlot { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null)
            {
                Inventory toInventory = Player.Inventory;

                Item toItem = toInventory.GetContent(ToSlot) as Item;

                if (toItem == null)
                {
                    //Act

                    RemoveItem(fromInventory, fromItem, server, context);

                    AddItem(toInventory, ToSlot, fromItem, server, context);
                }
            }
        }
    }
}