using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfGarlicDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> lumpOfGarlicDough = new HashSet<ushort>() { 9113 };

        private static HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private static ushort garlicBread = 9111;

        private static HashSet<ushort> bakingTrays = new HashSet<ushort>() { 2561 };

        private static ushort bakingTrayWithGarlicDough = 9115;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfGarlicDough.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, garlicBread, 1) );
                    } );
                }
                else if (bakingTrays.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.ToItem, bakingTrayWithGarlicDough, 1) );
                    } );
                }   
            }

            return next();
        }
    }
}