using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SealedDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> doors = new Dictionary<ushort, ushort>()
        {
            { 1223, 1224 },
            { 1225, 1226 },

            { 1241, 1242 },
            { 1243, 1244 },

            { 1255, 1256 },
            { 1257, 1258 },

            { 3542, 3543 },
            { 3551, 3552 },

            { 5105, 5106 },
            { 5114, 5115 },

            { 5123, 5124 },
            { 5132, 5133 },

            { 5288, 5289 },
            { 5290, 5291 },

            { 5745, 5746 },
            { 5748, 5749 },

            { 6202, 6203 },
            { 6204, 6205 },

            { 6259, 6260 },
            { 6261, 6262 },

            { 6898, 6899 },
            { 6907, 6908 },

            { 7040, 7041 },
            { 7049, 7050 },

            { 8551, 8552 },
            { 8553, 8554 },

            { 9175, 9176 },
            { 9177, 9178 },

            { 9277, 9278 },
            { 9279, 9280 },

            { 10278, 10279 },
            { 10280, 10281 },

            { 10475, 10476 },
            { 10484, 10485 },

            { 10782, 10783 },
            { 10791, 10792 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (doors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.ActionId < 1000)
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "The door seems to be sealed against unwanted intruders.") );

                    return Promise.Completed;
                }

                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    return Context.AddCommand(new CreatureWalkCommand(command.Player, (Tile)item.Parent) );
                } );
            }

            return next();
        }
    }
}