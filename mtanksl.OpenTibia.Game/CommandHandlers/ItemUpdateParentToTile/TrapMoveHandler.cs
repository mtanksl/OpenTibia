using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapMoveHandler : CommandHandler<ItemUpdateParentToTileCommand>
    {
        private Dictionary<ushort, ushort> traps = new Dictionary<ushort, ushort>()
        {
            { 2579, 2578 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, ItemUpdateParentToTileCommand command)
        {
            ushort toOpenTibiaId;

            if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new ItemDestroyCommand(command.Item) ).Then(ctx =>
                {
                    return ctx.AddCommand(new TileCreateItemCommand(command.ToTile, toOpenTibiaId, 1) );            
                } );
            }

            return next(context);
        }
    }
}