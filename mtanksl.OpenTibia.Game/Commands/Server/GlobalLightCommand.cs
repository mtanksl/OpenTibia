using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class GlobalLightCommand : Command
    {
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            server.Clock.Tick();

            //Notify

            IOutgoingPacket packet = new SetEnvironmentLightOutgoingPacket(server.Clock.Light);

            foreach (var player in server.Map.GetPlayers() )
            {
                context.Write(player.Client.Connection, packet);
            }

            server.QueueForExecution(Constants.GlobalLightSchedulerEvent, Constants.GlobalLightSchedulerEventInterval, this);
        }
    }
}