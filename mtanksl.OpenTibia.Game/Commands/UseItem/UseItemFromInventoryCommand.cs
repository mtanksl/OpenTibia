using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromInventoryCommand : UseItemCommand
    {
        public UseItemFromInventoryCommand(Player player, byte fromSlot)
        {
            Player = player;

            FromSlot = fromSlot;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null)
            {
                //Act

                Container container = fromItem as Container;

                if (container != null)
                {
                    OpenOrCloseContainer(Player, container, server, context);
                }
            }
        }
    }
}