using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BakingTrayWithGarlicDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> bakingTrayWithGarlicDough = new HashSet<ushort>() { 9115 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort garlicCookie = 9116;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (bakingTrayWithGarlicDough.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ItemDestroyCommand(command.Item) ).Then( () =>
                {
                    return Context.AddCommand(new TileIncrementOrCreateItemCommand( (Tile)command.ToItem.Parent, garlicCookie, 12) );
                } );
            }

            return next();
        }
    }
}