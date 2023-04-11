using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerRotateItemCommand : Command
    {
        public PlayerRotateItemCommand(Player player, Item item)
        {
            Player = player;

            Item = item;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

            return Promise.Break;
        }
    }
}