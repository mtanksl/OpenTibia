using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class JuiceSqueezerHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> juiceSqueezers = new HashSet<ushort>() { 5865, 10513 };

        private HashSet<ushort> fruits = new HashSet<ushort>() { 2676, 2677, 2684, 2679, 2681, 8841, 5097, 2672, 2675, 2673, 8839, 8840, 2674, 2680 };

        private HashSet<ushort> coconuts = new HashSet<ushort>() { 2678 };

        private ushort emptyVial = 11396;

        public override async Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (juiceSqueezers.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (fruits.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    int count = await Context.AddCommand(new PlayerCountItemsCommand(command.Player, emptyVial, (byte)FluidType.Empty) );

                    if (count > 0)
                    {
                        await Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) );

                        await Context.AddCommand(new PlayerDestroyItemsCommand(command.Player, emptyVial, (byte)FluidType.Empty, 1) );

                        await Context.AddCommand(new PlayerCreateItemCommand(command.Player, emptyVial, (byte)FluidType.FruitJuice) );
                    }
                    else
                    {
                        await next();
                    }
                }
                else if (coconuts.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    int count = await Context.AddCommand(new PlayerCountItemsCommand(command.Player, emptyVial, (byte)FluidType.Empty) );

                    if (count > 0)
                    {
                        await Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) );

                        await Context.AddCommand(new PlayerDestroyItemsCommand(command.Player, emptyVial, (byte)FluidType.Empty, 1) );

                        await Context.AddCommand(new PlayerCreateItemCommand(command.Player, emptyVial, (byte)FluidType.CoconutMilk) );
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}