using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GateOfExpertiseHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static Dictionary<ushort, ushort> doors = new Dictionary<ushort, ushort>()
        {
            { 1227, 1228 },
            { 1229, 1230 },

            { 1245, 1246 },
            { 1247, 1248 },

            { 1259, 1260 },
            { 1261, 1262 },

            { 3540, 3541 },
            { 3549, 3550 },

            { 5103, 5104 },
            { 5112, 5113 },

            { 5121, 5122 },
            { 5130, 5131 },

            { 5292, 5293 },
            { 5294, 5295 },

            { 6206, 6207 },
            { 6208, 6209 },

            { 6263, 6264 },
            { 6265, 6266 },

            { 6896, 6897 },
            { 6905, 6906 },

            { 7038, 7039 },
            { 7047, 7048 },

            { 8555, 8556 },
            { 8557, 8558 },

            { 9179, 9180 },
            { 9181, 9182 },

            { 9281, 9282 },
            { 9283, 9284 },

            { 10282, 10283 },
            { 10284, 10285 },

            { 10473, 10474 },
            { 10482, 10483 },

            { 10780, 10781 },
            { 10789, 10790 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (doors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.ActionId < 1000 || command.Player.Level < command.Item.ActionId - 1000)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Only the worthy may pass.") );

                    return Promise.Completed;
                }

                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    return Context.AddCommand(new CreatureMoveCommand(command.Player, (Tile)item.Parent) );
                } );
            }

            return next();
        }
    }
}