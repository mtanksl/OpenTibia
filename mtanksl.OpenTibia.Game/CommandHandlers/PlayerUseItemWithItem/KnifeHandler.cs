using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> knifes;
        private readonly HashSet<ushort> pumpkins;
        private readonly ushort pumpkinhead;
        private readonly HashSet<ushort> fruits;   
        private readonly ushort cake;
        private readonly ushort decoratedCake;

        public KnifeHandler()
        {
            knifes = Context.Server.Values.GetUInt16HashSet("values.items.knifes");
            pumpkins = Context.Server.Values.GetUInt16HashSet("values.items.pumpkins");
            pumpkinhead = Context.Server.Values.GetUInt16("values.items.pumpkinhead");
            fruits = Context.Server.Values.GetUInt16HashSet("values.items.fruits");
            cake = Context.Server.Values.GetUInt16("values.items.cake");
            decoratedCake = Context.Server.Values.GetUInt16("values.items.decoratedCake");
        }

        public override async Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (knifes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (pumpkins.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    await Context.AddCommand(new ItemTransformCommand(command.ToItem, pumpkinhead, 1) );
                }
                else if (fruits.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    int count = await Context.AddCommand(new PlayerCountItemsCommand(command.Player, cake, 1) );

                    if (count > 0)
                    {
                        await Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) );

                        await Context.AddCommand(new PlayerDestroyItemsCommand(command.Player, cake, 1, 1) );

                        await Context.AddCommand(new PlayerCreateItemCommand(command.Player, decoratedCake, 1) );

                        await Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.WithACherryOnTop, 20, "With a Cherry on Top") );
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