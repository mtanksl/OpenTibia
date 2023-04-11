using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WatchHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> watches = new HashSet<ushort>() { 2036 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (watches.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "The time is " + Context.Server.Clock.Hour.ToString("00") + ":" + Context.Server.Clock.Minute.ToString("00") + ".") );

                return Promise.Completed();
            }

            return next();
        }
    }
}