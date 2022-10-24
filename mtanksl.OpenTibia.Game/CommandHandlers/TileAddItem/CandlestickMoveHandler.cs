using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CandlestickMoveHandler : CommandHandler<TileAddItemCommand>
    {
        private HashSet<ushort> candlestick = new HashSet<ushort>() { 2048 };

        private Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            { 2096, 2097 },

            { 6279, 6280 },

            { 5813, 5812 }
        };

        public override Promise Handle(Context context, Func<Context, Promise> next, TileAddItemCommand command)
        {
            ushort toOpenTibiaId;

            if (candlestick.Contains(command.Item.Metadata.OpenTibiaId) && command.Tile.TopItem != null && transformations.TryGetValue(command.Tile.TopItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return context.AddCommand(new ItemTransformCommand(command.Tile.TopItem, toOpenTibiaId, 1) ).Then( (ctx, item) =>
                {
                    return ctx.AddCommand(new ItemDestroyCommand(command.Item) );          
                } ); 
            }

            return next(context);
        }
    }
}