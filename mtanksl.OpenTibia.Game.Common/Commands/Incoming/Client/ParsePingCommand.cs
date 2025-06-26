using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParsePingCommand : IncomingCommand
    {
        public ParsePingCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {                    
            Context.AddPacket(Player, new PingResponseOutgoingPacket() );

            return Promise.Completed;
        }
    }
}