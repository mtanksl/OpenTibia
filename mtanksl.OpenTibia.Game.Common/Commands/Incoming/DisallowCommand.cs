using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class DisallowCommand : IncomingCommand
    {
        public DisallowCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );

            return Promise.Completed;
        }
    }
}