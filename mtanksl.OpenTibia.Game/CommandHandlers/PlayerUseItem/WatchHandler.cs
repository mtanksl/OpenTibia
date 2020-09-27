using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WatchHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> watches = new HashSet<ushort>() { 2036 };

        public override bool CanHandle(PlayerUseItemCommand command, Server server)
        {
            if (watches.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Server server)
        {
            return new ConditionalCommand(context =>
            {
                context.WritePacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "The time is " + context.Server.Clock.Hour.ToString("00") + ":" + context.Server.Clock.Minute.ToString("00") + ".") );

                return true;
            } );
        }
    }
}