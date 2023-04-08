using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
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

            //TODO: More items
        };

        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            //TODO: The door seems to be sealed against unwanted intruders.

            ushort toOpenTibiaId;

            if (doors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                {
                    return ctx.AddCommand(new CreatureUpdateParentCommand(command.Player, (Tile)item.Parent) );
                } );
            }

            return next(context);
        }
    }
}