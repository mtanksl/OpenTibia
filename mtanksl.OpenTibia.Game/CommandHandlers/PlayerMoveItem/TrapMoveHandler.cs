using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapMoveHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        public override Promise Handle(ContextPromiseDelegate next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                ushort toOpenTibiaId;

                if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return context.AddCommand(new TileCreateItemCommand(tile, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                    {
                        return ctx.AddCommand(new ItemDestroyCommand(command.Item) );            
                    } );
                }
            }

            return next(context);
        }
    }
}