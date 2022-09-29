using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Commands
{
    public class SellNpcTradeCommand : Command
    {
        public SellNpcTradeCommand(Player player, SellNpcTradeIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public SellNpcTradeIncomingPacket Packet { get; set; }

        public override void Execute(Context context)
        {
            base.Execute(context);
        }
    }
}