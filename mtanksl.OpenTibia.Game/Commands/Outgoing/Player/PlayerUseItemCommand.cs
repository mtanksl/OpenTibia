using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUseItemCommand : Command
    {
        public PlayerUseItemCommand(Player player, Item item, byte? containerId)
        {
            Player = player;

            Item = item;

            ContainerId = containerId;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public byte? ContainerId { get; set; }

        public override void Execute(Context context)
        {
            context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );

            base.Execute(context);
        }
    }
}