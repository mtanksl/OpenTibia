using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ExplosivePresentHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> explosivePresents;

        public ExplosivePresentHandler()
        {
            explosivePresents = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.explosivePresents") );
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (explosivePresents.Contains(command.Item.Metadata.OpenTibiaId) )
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