using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStopWalkCommand : Command
    {
        public ParseStopWalkCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (Context.Server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) ) )
                {
                    Context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
                }

                Context.Server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(Player) );

                resolve();
            } );
        }
    }
}