using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfGarlicDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> lumpOfHolyWaterAndGarlicDough = new HashSet<ushort>() { 9113 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort garlicBread = 9111;

        private HashSet<ushort> bakingTrays = new HashSet<ushort>() { 2561 };

        private ushort bakingTrayWithGarlicDough = 9115;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfHolyWaterAndGarlicDough.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new TileIncrementOrCreateItemCommand( (Tile)command.ToItem.Parent, garlicBread, 1) );
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