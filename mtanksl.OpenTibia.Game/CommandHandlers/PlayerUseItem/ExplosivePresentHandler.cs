using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ExplosivePresentHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> explosivePresent = new HashSet<ushort>() { 8110 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (explosivePresent.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.JokesOnYou, 1, "Jokes On You") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.ExplosionDamage) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "KABOOOOOOOOOOM!") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}