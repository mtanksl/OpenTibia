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

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemWithCreatureCommand command)
        {
            if (manaPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                context.AddCommand(new CombatChangeManaCommand(null, player, null, context.Server.Randomization.Take(110, 190) ) );

                context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.BlueShimmer) );

                context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Aaaah...") );

                return Promise.Completed(context);
            }

            return next(context);
        }
    }
}