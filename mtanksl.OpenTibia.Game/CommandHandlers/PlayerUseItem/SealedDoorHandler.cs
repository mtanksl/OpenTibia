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
            // Brick
            { 5105, 5106 },
            { 5114, 5115 },

            // Framework
            { 1223, 1224 },
            { 1225, 1226 },

            // Pyramid
            { 1241, 1242 },
            { 1243, 1244 },

            // White stone
            { 1255, 1256 },
            { 1257, 1258 },

            // Stone
            { 5123, 5124 },
            { 5132, 5133 },

            // Stone
            { 6259, 6260 },
            { 6261, 6262 },

            //TODO: More items
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