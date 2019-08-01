using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class GlobalLightCommand : Command
    {
        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            server.Clock.Tick();

            //Notify

            foreach (var player in server.Map.GetPlayers() )
            {
                context.Write(player.Client.Connection, new SetEnvironmentLightOutgoingPacket(server.Clock.Light) );
            }

            server.QueueForExecution(Constants.GlobalLightSchedulerEvent, Constants.GlobalLightSchedulerEventInterval, this);
        }
    }
}