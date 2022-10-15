using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FireworksRocketHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> fireworksRockets = new HashSet<ushort>() { 6576 };

        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            if (fireworksRockets.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (command.Item.Parent is Tile tile)
                {
                    context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.FireworkBlue) );
                }
                else
                {
                    context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.ExplosionDamage) );

                    context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Ouch! Rather place it on the ground next time.") );

                    context.AddCommand(new CombatChangeHealthCommand(null, command.Player, -10) );
                }

                context.AddCommand(new ItemDestroyCommand(command.Item) );

                return Promise.FromResult(context);
            }

            return next(context);
        }
    }
}