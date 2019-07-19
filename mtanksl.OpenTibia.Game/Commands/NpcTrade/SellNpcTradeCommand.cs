using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Commands
{
    public class SellNpcTradeCommand : Command
    {
        public SellNpcTradeCommand(Player player, SellNpcTradeIncommingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public SellNpcTradeIncommingPacket Packet { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange



            //Act



            //Notify


        }
    }
}