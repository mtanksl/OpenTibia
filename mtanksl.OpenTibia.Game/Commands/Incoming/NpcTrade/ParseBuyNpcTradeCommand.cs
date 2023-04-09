using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Commands
{
    public class ParseBuyNpcTradeCommand : Command
    {
        public ParseBuyNpcTradeCommand(Player player, BuyNpcTradeIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public BuyNpcTradeIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {


                resolve(context);
            } );
        }
    }
}