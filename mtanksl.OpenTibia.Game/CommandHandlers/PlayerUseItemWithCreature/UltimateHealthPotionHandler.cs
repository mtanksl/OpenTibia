using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UltimateHealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private static HashSet<ushort> healthPotions = new HashSet<ushort>() { 8473 };

        private static ushort emptyPotionFlask = 7635;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (healthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                if (player.Level < 130 || !(player.Vocation == Vocation.Knight || player.Vocation == Vocation.EliteKnight) )
                {
                    return Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Only knights of level 130 or above may drink this fluid.") );
                }

                Promise promise;

                if (Context.Current.Server.Config.GameplayInfinitePotions)
                {
                    promise = Promise.Completed;
                }
                else
                {
                    promise = Context.Current.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, emptyPotionFlask, 1) );
                    } );
                }

                return promise.Then( () =>
                {
                    return Context.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + Context.Server.Randomization.Take(800, 1000) ) );
                    
                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.RedShimmer) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Aaaah...") );
                } );
            }

            return next();
        }
    }
}