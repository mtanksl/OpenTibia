using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfHolyWaterDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> lumpOfHolyWaterDough = new HashSet<ushort>() { 9112 };

        private HashSet<ushort> garlic = new HashSet<ushort>() { 9114 };

        private ushort lumpOfHolyWaterAndGarlicDough = 9113;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfHolyWaterDough.Contains(command.Item.Metadata.OpenTibiaId) && garlic.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new PlayerInventoryContainerTileCreateItemCommand(command.Player, lumpOfHolyWaterAndGarlicDough, 1) );
                } );
            }

            return next();
        }
    }
}