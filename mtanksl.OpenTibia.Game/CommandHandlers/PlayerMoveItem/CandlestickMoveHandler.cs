using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CandlestickMoveHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly HashSet<ushort> candlesticks;
        private readonly Dictionary<ushort, ushort> transformations;

        public CandlestickMoveHandler()
        {
            candlesticks = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.candlesticks") );
            transformations = LuaScope.GetInt16Int16Dictionary(Context.Server.Values.GetValue("values.items.transformation.candlesticks") );
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                ushort toOpenTibiaId;

                if (candlesticks.Contains(command.Item.Metadata.OpenTibiaId) && tile.TopItem != null && transformations.TryGetValue(tile.TopItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
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