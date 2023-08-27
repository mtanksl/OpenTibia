using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerTradeWithCommand : Command
    {
        public PlayerTradeWithCommand(Player player, Item item, Player toPlayer)
        {
            Player = player;

            Item = item;

            ToPlayer = toPlayer;
        }

        public Player Player { get; }

        public Item Item { get; }

        public Player ToPlayer { get; }

        public override Promise Execute()
        {
            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

            return Promise.Break;
        }
    }
}