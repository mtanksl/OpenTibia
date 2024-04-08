using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FluidItem2Handler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> drawWell = new HashSet<ushort> { 1368, 1369 };

        private static HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4820, 4821, 4822, 4823, 4824, 4825 };

        private static HashSet<ushort> swamps = new HashSet<ushort>() { 4691, 4692, 4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712 };

        private static HashSet<ushort> lavas = new HashSet<ushort>() { 598, 599, 600, 601 };

        private static HashSet<ushort> distillingMachines = new HashSet<ushort>() { 5513, 5514 };

        private static HashSet<ushort> waterCask = new HashSet<ushort> { 1771 };

        private static HashSet<ushort> beerCask = new HashSet<ushort> { 1772 };

        private static HashSet<ushort> wineCask = new HashSet<ushort> { 1773 };

        private static HashSet<ushort> lemonadeCask = new HashSet<ushort> { 1776 };

        private static HashSet<ushort> rumCask = new HashSet<ushort> { 5539 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (command.Item is FluidItem fromItem)
            {
                if (fromItem.FluidType == FluidType.Empty)
                {
                    if (drawWell.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Water) );
                    }
                    else if (shallowWaters.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.BlueRings) ).Then( () =>
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Water) );
                        } );
                    }
                    else if (swamps.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.GreenRings) ).Then( () =>
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Slime) );
                        } );
                    }
                    else if (lavas.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.FirePlume) ).Then( () =>
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Lava) );
                        } );
                    }
                    else if (distillingMachines.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Rum) );
                    }
                    else if (waterCask.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Water) );
                    }
                    else if (beerCask.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Beer) );
                    }
                    else if (wineCask.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Wine) );
                    }
                    else if (lemonadeCask.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Lemonade) );
                    }
                    else if (rumCask.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Rum) );
                    }
                }
                else
                {
                    if (command.ToItem is FluidItem toItem)
                    { 
                        if (toItem.FluidType == FluidType.Empty)
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(toItem, fromItem.FluidType) ).Then( () =>
                            {
                                return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) );
                            } );
                        }
                    }
                }
            }

            return next();
        }
    }
}