using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CandlestickMoveHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private static HashSet<ushort> candlestick = new HashSet<ushort>() { 2048 };

        private static Dictionary<ushort, ushort> transformations = new Dictionary<ushort, ushort>()
        {
            // Pumkinhead
            { 2096, 2097 },

            // Cake
            { 6279, 6280 },

            // Skull candle
            { 5813, 5812 }
        };

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                ushort toOpenTibiaId;

                if (candlestick.Contains(command.Item.Metadata.OpenTibiaId) && tile.TopItem != null && transformations.TryGetValue(tile.TopItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new ItemTransformCommand(tile.TopItem, toOpenTibiaId, 1) ).Then( (item) =>
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) );          
                    } ); 
                }
            }

            return next();
        }
    }
}