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
        private readonly HashSet<ushort> watches;

        public WatchHandler()
        {
            watches = Context.Server.Values.GetUInt16HashSet("values.items.watches");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (watches.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "The time is " + Context.Server.Clock.Hour.ToString("00") + ":" + Context.Server.Clock.Minute.ToString("00") + ".") );

                return Promise.Completed;
            }

            return next();
        }
    }
}