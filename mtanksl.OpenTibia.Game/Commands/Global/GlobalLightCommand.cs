using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class GlobalLightCommand : Command
    {
        public override Promise Execute()
        {         
            Context.Server.Clock.Tick();

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                Context.AddPacket(observer.Client.Connection, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );
            }

            return Promise.Completed;
        }
    }
}