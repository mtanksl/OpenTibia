using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GateOfExpertiseHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> doors = new Dictionary<ushort, ushort>()
        {
            // Brick
            { 5103, 5104 },
            { 5112, 5113 },

            // Framework
            { 1227, 1228 },
            { 1229, 1230 },

            // Pyramid
            { 1245, 1246 },
            { 1247, 1248 },

            // White stone
            { 1259, 1260 },
            { 1261, 1262 },

            // Stone
            { 5121, 5122 },
            { 5130, 5131 },

            //TODO: More items
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            //TODO: Only the worthy may pass.

            ushort toOpenTibiaId;

            if (doors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    return Context.AddCommand(new CreatureUpdateTileCommand(command.Player, (Tile)item.Parent) );
                } );
            }

            return next();
        }
    }
}