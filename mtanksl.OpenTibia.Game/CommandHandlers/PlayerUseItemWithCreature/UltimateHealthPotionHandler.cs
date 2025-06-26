using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UltimateHealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private readonly HashSet<ushort> ultimateHealthPotions;
        private readonly ushort greatEmptyPotionFlask;

        public UltimateHealthPotionHandler()
        {
            ultimateHealthPotions = Context.Server.Values.GetUInt16HashSet("values.items.ultimateHealthPotions");
            greatEmptyPotionFlask = Context.Server.Values.GetUInt16("values.items.greatEmptyPotionFlask");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (ultimateHealthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                if (player.Level < 130 || !(player.Vocation == Vocation.Knight || player.Vocation == Vocation.EliteKnight) )
                {
                    return Context.AddCommand(new ShowTextCommand(player, MessageMode.MonsterSay, "Only knights of level 130 or above may drink this fluid.") );
                }

                Promise promise;

                if ( !Context.Server.Config.GameplayRemoveChargesFromPotions)
                {
                    promise = Promise.Completed;
                }
                else
                {
                    promise = Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, greatEmptyPotionFlask, 1) );
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
                    return Context.AddCommand(new ShowTextCommand(player, MessageMode.MonsterSay, "Aaaah...") );
                } );
            }

            return next();
        }
    }
}