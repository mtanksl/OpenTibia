using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapMoveHandler : CommandHandler<TileAddItemCommand>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, TileAddItemCommand command)
        {
            ushort toOpenTibiaId;

            if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new TileCreateItemCommand(command.Tile, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                {
                    return ctx.AddCommand(new ItemDestroyCommand(command.Item) );            
                } );
            }

            return next(context);
        }
    }
}