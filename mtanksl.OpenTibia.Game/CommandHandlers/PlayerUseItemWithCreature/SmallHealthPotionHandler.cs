using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SmallHealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private readonly HashSet<ushort> smallHealthPotions;
        private readonly ushort smallEmptyPotionFlask;

        public SmallHealthPotionHandler()
        {
            smallHealthPotions = Context.Server.Values.GetUInt16HashSet("values.items.smallHealthPotions");
            smallEmptyPotionFlask = Context.Server.Values.GetUInt16("values.items.smallEmptyPotionFlask");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (smallHealthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
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
                    return Context.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + Context.Server.Randomization.Take(50, 100) ) ); 
                    
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