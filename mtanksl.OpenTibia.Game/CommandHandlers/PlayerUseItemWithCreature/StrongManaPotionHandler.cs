using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StrongManaPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private HashSet<ushort> manaPotions = new HashSet<ushort>() { 7589 };

        private ushort emptyPotionFlask = 7634;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (manaPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                if (player.Level < 50 || !(player.Vocation == Vocation.Paladin || player.Vocation == Vocation.Druid || player.Vocation == Vocation.Sorcerer || player.Vocation == Vocation.RoyalPaladin || player.Vocation == Vocation.ElderDruid || player.Vocation == Vocation.MasterSorcerer) )
                {
                    return Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Only sorcerers, druids and paladins of level 50 or above may drink this fluid.") );
                }

                Tile toTile = player.Tile;

                Promise promise;

                if (Context.Current.Server.Config.GamePlayInfinitePotions)
                {
                    promise = Promise.Completed;
                }
                else
                {
                    promise = Context.Current.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerInventoryContainerTileCreateItemCommand(command.Player, emptyPotionFlask, 1) );
                    } );
                }

                return promise.Then( () =>
                {
                    return Context.AddCommand(new PlayerUpdateManaCommand(player, (ushort)(player.Mana + Context.Server.Randomization.Take(110, 190) ) ) );
                    
                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Aaaah...") );
                } );
            }

            return next();
        }
    }
}