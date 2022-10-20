using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class CombatConditionCommand : Command
    {
        public CombatConditionCommand(Creature target, MagicEffectType magicEffectType, int[] health, int[] cooldownInMilliseconds)
        {
            Target = target;

            MagicEffectType = magicEffectType;

            Health = health;

            CooldownInMilliseconds = cooldownInMilliseconds;
        }

        public Creature Target { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public int[] Health { get; set; }

        public int[] CooldownInMilliseconds { get; set; }

        public override Promise Execute(Context context)
        {
            List<Command> commands = new List<Command>();

            for (int i = 0; i < Health.Length; i++)
            {
                int j = i;

                commands.Add(new CombatTargetedAttackCommand(null, Target, null, MagicEffectType, (attacker, target) => Health[j] ) );

                if (j < Health.Length - 1)
                {
                    commands.Add(new InlineCommand(ctx => ctx.Server.Components.AddComponent(Target, new DecayBehaviour(CooldownInMilliseconds[j] ) ).Promise) );
                }
            }

            return context.AddCommand(new SequenceCommand(commands.ToArray() ) );
        }
    }
}