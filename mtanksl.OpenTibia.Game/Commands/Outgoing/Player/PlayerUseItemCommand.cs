using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;

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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
            } );
        }
    }
}