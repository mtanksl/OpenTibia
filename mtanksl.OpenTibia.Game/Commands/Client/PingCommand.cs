using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PingCommand : Command
    {
        public PingCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange
            
            

            //Act

            

            //Notify

            context.Write(Player.Client.Connection, new PingOutgoingPacket() );
        }
    }
}