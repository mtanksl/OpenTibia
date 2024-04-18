using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> knifes = new HashSet<ushort>() { 2566, 10515, 10511 };

        private static HashSet<ushort> pumpkins = new HashSet<ushort>() { 2683 };

        private static ushort pumpkinhead = 2096;

        private static HashSet<ushort> fruits = new HashSet<ushort>() { 2676, 2677, 2684, 2679, 2678, 2681, 8841, 5097, 2672, 2675, 2673, 8839, 8840, 2674, 2680 };

        private static ushort cake = 6278;

        private static ushort decoratedCake = 6279;

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