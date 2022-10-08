using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class GlobalLightCommand : Command
    {
        public override Promise Execute(Context context)
        {         
            return Promise.Run(resolve =>
            {
                context.Server.Clock.Tick();

                foreach (var player in context.Server.GameObjects.GetPlayers() )
                {
                    context.AddPacket(player.Client.Connection, new SetEnvironmentLightOutgoingPacket(context.Server.Clock.Light) );
                }

                resolve(context);
            } );
        }
    }
}