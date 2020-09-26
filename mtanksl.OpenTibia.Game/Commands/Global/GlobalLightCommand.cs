using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class GlobalLightCommand : Command
    {
        public override void Execute(Context context)
        {                        
            context.Server.Clock.Tick();

            foreach (var player in context.Server.GameObjects.GetPlayers())
            {
                context.AddPacket(player.Client.Connection, new SetEnvironmentLightOutgoingPacket(context.Server.Clock.Light) );
            }

            base.Execute(context);
        }
    }
}