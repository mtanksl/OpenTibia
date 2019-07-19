using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Commands
{
    public class BuyNpcTradeCommand : Command
    {
        public BuyNpcTradeCommand(Player player, BuyNpcTradeIncommingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public BuyNpcTradeIncommingPacket Packet { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange



            //Act



            //Notify


        }
    }
}