using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GreatHealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private readonly HashSet<ushort> greatHealthPotions;
        private readonly ushort greatEmptyPotionFlask;

        public GreatHealthPotionHandler()
        {
            greatHealthPotions = Context.Server.Values.GetUInt16HashSet("values.items.greatHealthPotions");
            greatEmptyPotionFlask = Context.Server.Values.GetUInt16("values.items.greatEmptyPotionFlask");            
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (greatHealthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                if (player.Level < 80 || !(player.Vocation == Vocation.Knight || player.Vocation == Vocation.EliteKnight) )
                {
                    return Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Only knights of level 80 or above may drink this fluid.") );
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
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, greatEmptyPotionFlask, 1) );
                    } );
                }

                return promise.Then( () =>
                {
                    return Context.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + Context.Server.Randomization.Take(500, 700) ) );  
                    
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