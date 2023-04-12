using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Commands
{
    public class ParseSellNpcTradeCommand : Command
    {
        public ParseSellNpcTradeCommand(Player player, SellNpcTradeIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public SellNpcTradeIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            return Promise.Completed;
        }
    }
}