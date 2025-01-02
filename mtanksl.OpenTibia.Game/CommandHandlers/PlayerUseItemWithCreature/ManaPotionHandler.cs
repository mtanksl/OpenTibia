using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ManaPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private readonly HashSet<ushort> manaPotions;
        private readonly ushort smallEmptyPotionFlask;

        public ManaPotionHandler()
        {
            manaPotions = Context.Server.Values.GetUInt16HashSet("values.items.manaPotions");
            smallEmptyPotionFlask = Context.Server.Values.GetUInt16("values.items.smallEmptyPotionFlask");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (manaPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {   
                Promise promise;

                if ( !Context.Current.Server.Config.GameplayRemoveChargesFromPotions)
                {
                    promise = Promise.Completed;
                }
                else
                {
                    promise = Context.Current.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, smallEmptyPotionFlask, 1) );
                    } );
                }

                return promise.Then( () =>
                {
                    return Context.AddCommand(new PlayerUpdateManaCommand(player, (ushort)(player.Mana + Context.Server.Randomization.Take(70, 130) ) ) );  
                    
                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Aaaah...") );
                } );
            }

            return next();
        }
    }
}