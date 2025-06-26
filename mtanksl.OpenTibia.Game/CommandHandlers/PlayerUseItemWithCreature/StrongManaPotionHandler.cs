using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StrongManaPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private readonly HashSet<ushort> strongManaPotions;
        private readonly ushort strongEmptyPotionFlask;

        public StrongManaPotionHandler()
        {
            strongManaPotions = Context.Server.Values.GetUInt16HashSet("values.items.strongManaPotions");
            strongEmptyPotionFlask = Context.Server.Values.GetUInt16("values.items.strongEmptyPotionFlask");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (strongManaPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                if (player.Level < 50 || !(player.Vocation == Vocation.Paladin || player.Vocation == Vocation.Druid || player.Vocation == Vocation.Sorcerer || player.Vocation == Vocation.RoyalPaladin || player.Vocation == Vocation.ElderDruid || player.Vocation == Vocation.MasterSorcerer) )
                {
                    return Context.AddCommand(new ShowTextCommand(player, MessageMode.MonsterSay, "Only sorcerers, druids and paladins of level 50 or above may drink this fluid.") );
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
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, strongEmptyPotionFlask, 1) );
                    } );
                }

                return promise.Then( () =>
                {
                    return Context.AddCommand(new PlayerUpdateManaCommand(player, player.Mana + Context.Server.Randomization.Take(110, 190) ) );
                    
                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(player, MessageMode.MonsterSay, "Aaaah...") );
                } );
            }

            return next();
        }
    }
}