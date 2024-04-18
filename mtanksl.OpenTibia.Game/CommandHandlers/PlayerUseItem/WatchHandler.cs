using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WatchHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> watches = new HashSet<ushort>() { 1728, 1729, 1730, 1731, 1873, 1874, 1875, 1876, 1877, 1881, 2036, 3900, 7828, 9235, 9236, 9237, 9238 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (watches.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "The time is " + Context.Server.Clock.Hour.ToString("00") + ":" + Context.Server.Clock.Minute.ToString("00") + ".") );

                return Promise.Completed;
            }

            return next();
        }
    }
}