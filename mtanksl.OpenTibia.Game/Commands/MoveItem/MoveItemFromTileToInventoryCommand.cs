using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromTileToInventoryCommand(Player player, Position fromPosition, byte fromIndex, byte toSlot)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ToSlot = toSlot;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public byte ToSlot { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange



            //Act



            //Notify


        }
    }
}