using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;
using System;

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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}