using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToTileCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToTileCommand(Player player, byte fromSlot, Position toPosition)
        {
            Player = player;

            FromSlot = fromSlot;

            ToPosition = toPosition;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public Position ToPosition { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange



            //Act



            //Notify


        }
    }
}