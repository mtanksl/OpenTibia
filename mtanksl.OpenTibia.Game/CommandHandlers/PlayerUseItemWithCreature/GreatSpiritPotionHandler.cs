using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GreatSpiritPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private HashSet<ushort> manaPotions = new HashSet<ushort>() { 8472 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (manaPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player player)
            {
                Context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                Context.AddCommand(new CombatChangeHealthCommand(null, player, null, Context.Server.Randomization.Take(200, 400) ) );

                Context.AddCommand(new CombatChangeManaCommand(null, player, null, Context.Server.Randomization.Take(110, 190) ) );

                Context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.BlueShimmer) );

                Context.AddCommand(new ShowTextCommand(player, TalkType.MonsterSay, "Aaaah...") );

                return Promise.Completed;
            }

            return next();
        }
    }
}