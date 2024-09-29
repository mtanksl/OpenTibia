using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FlourHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> flours;
        private readonly HashSet<ushort> buckets;
        private readonly HashSet<ushort> holyWaters;
        private readonly ushort lumpOfDough;
        private readonly ushort lumpOfCakeDough;
        private readonly ushort lumpOfHolyWaterDough;
        private readonly ushort emptyVial;

        public FlourHandler()
        {
            flours = Context.Server.Values.GetUInt16HashSet("values.items.flours");
            buckets = Context.Server.Values.GetUInt16HashSet("values.items.buckets");
            holyWaters = Context.Server.Values.GetUInt16HashSet("values.items.holyWaters");
            lumpOfDough = Context.Server.Values.GetUInt16("values.items.lumpOfDough");
            lumpOfCakeDough = Context.Server.Values.GetUInt16("values.items.lumpOfCakeDough");
            lumpOfHolyWaterDough = Context.Server.Values.GetUInt16("values.items.lumpOfHolyWaterDough");
            emptyVial = Context.Server.Values.GetUInt16("values.items.emptyVial");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (flours.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (buckets.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    FluidItem toFluidItem = (FluidItem)command.ToItem;

                    if (toFluidItem.FluidType == FluidType.Water)
                    {
                        return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, lumpOfDough, 1) );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(toFluidItem, FluidType.Empty) );
                        } );
                    }
                    else if (toFluidItem.FluidType == FluidType.Milk)
                    {
                        return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                        {
                            return Context.AddCommand(new PlayerCreateItemCommand(command.Player, lumpOfCakeDough, 1) );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(toFluidItem, FluidType.Empty) );
                        } );
                    }
                }
                else if (holyWaters.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, lumpOfHolyWaterDough, 1) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.ToItem, emptyVial, 0) );
                    } );
                }
            }

            return next();
        }
    }
}