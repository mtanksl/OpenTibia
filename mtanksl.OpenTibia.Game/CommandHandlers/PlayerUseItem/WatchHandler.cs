using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WatchHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> watches = new HashSet<ushort>() { 2036 };

        public override bool CanHandle(PlayerUseItemCommand command, Context context)
        {
            if (watches.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Context context)
        {
            return new ConditionalCommand(_context =>
            {
                context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "The time is " + _context.Server.Clock.Hour.ToString("00") + ":" + _context.Server.Clock.Minute.ToString("00") + ".") );

                return true;
            } );
        }
    }
}