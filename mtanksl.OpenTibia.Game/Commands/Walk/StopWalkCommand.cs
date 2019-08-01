using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class StopWalkCommand : Command
    {
        public StopWalkCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            if (server.CancelQueueForExecution(Constants.PlayerSchedulerEvent(Player) ) )
            {
                //Notify

                context.Write(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
            }

            base.Execute(server, context);
        }
    }
}