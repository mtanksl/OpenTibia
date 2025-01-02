using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StrongHealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private readonly HashSet<ushort> strongHealthPotions;
        private readonly ushort strongEmptyPotionFlask;

        public StrongHealthPotionHandler()
        {
            strongHealthPotions = Context.Server.Values.GetUInt16HashSet("values.items.strongHealthPotions");
            strongEmptyPotionFlask = Context.Server.Values.GetUInt16("values.items.strongEmptyPotionFlask");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (strongHealthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                if (player.Level < 50 || !(player.Vocation == Vocation.Knight || player.Vocation == Vocation.Paladin || player.Vocation == Vocation.EliteKnight || player.Vocation == Vocation.RoyalPaladin) )
                {
                    return Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Only knights and paladins of level 50 or above may drink this fluid.") );
                }

                Promise promise;

                if ( !Context.Current.Server.Config.GameplayRemoveChargesFromPotions)
                {
                    promise = Promise.Completed;
                }
                else
                {
                    promise = Context.Current.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, strongEmptyPotionFlask, 1) );
                    } );
                }

                return promise.Then( () =>
                {
                    return Context.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + Context.Server.Randomization.Take(200, 400) ) );
                    
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