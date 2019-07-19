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

            Item fromItem = Player.Inventory.GetContent(FromSlot) as Item;

            if (fromItem != null)
            {
                //Act

                if (fromItem is Container container)
                {
                    OpenOrCloseContainer(Player, container, server, context);
                }
            }
        }
    }
}