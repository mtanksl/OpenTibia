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
                
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (context.Server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) ) )
                {
                    context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
                }

                context.Server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(Player) );

                resolve(context);
            } );
        }
    }
}