using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class JuiceSqueezerHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> juiceSqueezers;
        private readonly HashSet<ushort> fruits;
        private readonly HashSet<ushort> coconuts;
        private readonly ushort emptyVial;

        public JuiceSqueezerHandler()
        {
            juiceSqueezers = Context.Server.Values.GetUInt16HashSet("values.items.juiceSqueezers");
            fruits = Context.Server.Values.GetUInt16HashSet("values.items.fruits");
            coconuts = Context.Server.Values.GetUInt16HashSet("values.items.coconuts");
            emptyVial = Context.Server.Values.GetUInt16("values.items.emptyVial");
        }

        public override async Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (juiceSqueezers.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (coconuts.Contains(command.ToItem.Metadata.OpenTibiaId) )
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
                else if (fruits.Contains(command.ToItem.Metadata.OpenTibiaId) )
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