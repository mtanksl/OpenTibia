using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BakingTrayWithDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> bakingTrayWithDough = new HashSet<ushort>() { 8848 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort cookie = 2687;

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemWithItemCommand command)
        {
            if (bakingTrayWithDough.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new ItemDestroyCommand(command.Item) ).Then(ctx =>
                {
                    return ctx.AddCommand(new TileIncrementOrCreateItemCommand( (Tile)command.ToItem.Parent, cookie, 12) );
                } );
            }

            return next(context);
        }
    }
}