using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromInventoryCommand : LookCommand
    {
        public LookFromInventoryCommand(Player player, byte fromSlot)
        {
            Player = player;

            FromSlot = fromSlot;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act
            
            

            //Notify


        }
    }
}