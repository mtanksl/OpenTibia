using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromInventoryToInventoryCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromInventoryToInventoryCommand(Player player, byte fromSlot, byte toSlot)
        {
            Player = player;

            FromSlot = fromSlot;

            ToSlot = toSlot;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public byte ToSlot { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act
            
            

            //Notify


        }
    }
}