using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CandlestickMoveHandler : CommandHandler<ItemUpdateParentToTileCommand>
    {
        private HashSet<ushort> candlestick = new HashSet<ushort>() { 2048 };

        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 2096, 2097 },

            { 6279, 6280 },

            { 5813, 5812 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, ItemUpdateParentToTileCommand command)
        {
            ushort toOpenTibiaId;

            if (candlestick.Contains(command.Item.Metadata.OpenTibiaId) && command.ToTile.TopItem != null && transformations.TryGetValue(command.ToTile.TopItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new ItemDestroyCommand(command.Item) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemTransformCommand(command.ToTile.TopItem, toOpenTibiaId, 1) );          
                } ); 
            }

            return next(context);
        }
    }
}