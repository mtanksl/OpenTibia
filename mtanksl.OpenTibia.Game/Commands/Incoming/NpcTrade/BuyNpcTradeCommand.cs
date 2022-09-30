using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Commands
{
    public class BuyNpcTradeCommand : Command
    {
        public BuyNpcTradeCommand(Player player, BuyNpcTradeIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public BuyNpcTradeIncomingPacket Packet { get; set; }

        public override void Execute(Context context)
        {
            OnComplete(context);
        }
    }
}