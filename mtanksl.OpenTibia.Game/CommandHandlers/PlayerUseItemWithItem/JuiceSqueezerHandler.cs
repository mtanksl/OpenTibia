using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class JuiceSqueezerHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> juiceSqueezers = new HashSet<ushort>() { 5865, 10513 };

        private HashSet<ushort> fruits = new HashSet<ushort>() { 2676, 2677, 2684, 2679, 2678, 2681, 8841, 5097, 2672, 2675, 2673, 8839, 8840, 2674, 2680 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (juiceSqueezers.Contains(command.Item.Metadata.OpenTibiaId) && fruits.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                FluidItem item = command.Player.Inventory.GetContent( (byte)Slot.Extra) as FluidItem;

                if (item != null)
                {
                    if (item.FluidType == FluidType.Empty)
                    {
                        return Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) ).Then( () =>
                        {
                            return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(item, FluidType.FruitJuice) );
                        } );
                    }
                }
            }

            return next();
        }
    }
}