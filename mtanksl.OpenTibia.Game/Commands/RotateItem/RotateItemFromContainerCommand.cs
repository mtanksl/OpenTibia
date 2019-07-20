using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class RotateItemFromContainerCommand : RotateItemCommand
    {
        public RotateItemFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = ItemId;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}