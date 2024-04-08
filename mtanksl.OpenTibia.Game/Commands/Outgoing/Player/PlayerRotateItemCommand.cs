using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerRotateItemCommand : Command
    {
        public PlayerRotateItemCommand(IncomingCommand source, Player player, Item item)
        {
            Source = source;

            Player = player;

            Item = item;
        }

        public IncomingCommand Source { get; set; }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

            return Promise.Break;
        }
    }
}