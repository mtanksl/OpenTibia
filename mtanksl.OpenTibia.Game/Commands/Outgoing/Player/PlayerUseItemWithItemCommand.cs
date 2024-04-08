using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUseItemWithItemCommand : Command
    {
        public PlayerUseItemWithItemCommand(IncomingCommand source, Player player, Item item, Item toItem)
        {
            Source = source;

            Player = player;

            Item = item;

            ToItem = toItem;
        }

        public IncomingCommand Source { get; set; }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public Item ToItem { get; set; }

        public override Promise Execute()
        {
            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

            return Promise.Break;
        }
    }
}