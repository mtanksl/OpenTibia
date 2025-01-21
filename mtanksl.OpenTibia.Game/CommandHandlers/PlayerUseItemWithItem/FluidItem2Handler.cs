using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FluidItem2Handler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> drawWells;
        private readonly HashSet<ushort> shallowWaters;
        private readonly HashSet<ushort> swamps;
        private readonly HashSet<ushort> lavas;
        private readonly HashSet<ushort> distillingMachines;
        private readonly HashSet<ushort> waterCasks;
        private readonly HashSet<ushort> beerCasks;
        private readonly HashSet<ushort> wineCasks;
        private readonly HashSet<ushort> lemonadeCasks;
        private readonly HashSet<ushort> rumCasks;

        public FluidItem2Handler()
        {
            drawWells = Context.Server.Values.GetUInt16HashSet("values.items.drawWells");
            shallowWaters = Context.Server.Values.GetUInt16HashSet("values.items.shallowWaters");
            swamps = Context.Server.Values.GetUInt16HashSet("values.items.swamps");
            lavas = Context.Server.Values.GetUInt16HashSet("values.items.lavas");
            distillingMachines = Context.Server.Values.GetUInt16HashSet("values.items.distillingMachines");
            waterCasks = Context.Server.Values.GetUInt16HashSet("values.items.waterCasks");
            beerCasks = Context.Server.Values.GetUInt16HashSet("values.items.beerCasks");
            wineCasks = Context.Server.Values.GetUInt16HashSet("values.items.wineCasks");
            lemonadeCasks = Context.Server.Values.GetUInt16HashSet("values.items.lemonadeCasks");
            rumCasks = Context.Server.Values.GetUInt16HashSet("values.items.rumCasks");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (command.Item is FluidItem fromItem)
            {
                if (fromItem.FluidType == FluidType.Empty)
                {
                    if (drawWells.Contains(command.ToItem.Metadata.OpenTibiaId) )
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
                        return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.FireDamage) ).Then( () =>
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Lava) );
                        } );
                    }
                    else if (distillingMachines.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Rum) );
                    }
                    else if (waterCasks.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Water) );
                    }
                    else if (beerCasks.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Beer) );
                    }
                    else if (wineCasks.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Wine) );
                    }
                    else if (lemonadeCasks.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Lemonade) );
                    }
                    else if (rumCasks.Contains(command.ToItem.Metadata.OpenTibiaId) )
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