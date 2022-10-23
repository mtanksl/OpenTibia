using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class GlobalLightCommand : Command
    {
        public override Promise Execute(Context context)
        {         
            return Promise.Run(resolve =>
            {
                context.Server.Clock.Tick();

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    context.AddPacket(observer.Client.Connection, new SetEnvironmentLightOutgoingPacket(context.Server.Clock.Light) );
                }

                resolve(context);
            } );
        }
    }
}