using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FlourHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> flours = new HashSet<ushort>() { 2692 };

        private HashSet<ushort> buckets = new HashSet<ushort>() { 1775, 2005 };

        private ushort lumpOfDough = 2693;

        private ushort lumpOfCakeDough = 6277;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (flours.Contains(command.Item.Metadata.OpenTibiaId) && buckets.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                FluidItem toFluidItem = (FluidItem)command.ToItem;

                if (toFluidItem.FluidType == FluidType.Water)
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(toFluidItem, FluidType.Empty) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new TileIncrementOrCreateItemCommand(command.Player.Tile, lumpOfDough, 1) );
                    } );
                }
                else if (toFluidItem.FluidType == FluidType.Milk)
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(toFluidItem, FluidType.Empty) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new TileIncrementOrCreateItemCommand(command.Player.Tile, lumpOfCakeDough, 1) );
                    } );
                }
            }

            return next();
        }
    }
}