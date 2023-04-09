using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StuffedDragonHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> stuffedDragons = new HashSet<ushort>() { 5791 };

        private List<string> sounds = new List<string>() { "Fchhhhhh!", "Zchhhhhh!", "Grooaaaaar*cough*", "Aaa... CHOO!", "You... will.... burn!!" };

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            if (stuffedDragons.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = context.Server.Randomization.Take(0, sounds.Count);

                context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sounds[value] ) );

                if (value == sounds.Count - 1)
                {
                    context.AddCommand(CombatCommand.TargetAttack(null, command.Player, null, MagicEffectType.ExplosionDamage, (attacker, target) => -1) );
                }

                return Promise.Completed(context);
            }

            return next(context);
        }
    }
}