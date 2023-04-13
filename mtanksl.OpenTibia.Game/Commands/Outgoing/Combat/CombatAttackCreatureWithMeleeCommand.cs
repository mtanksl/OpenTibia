using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureWithMeleeCommand : Command
    {
        public CombatAttackCreatureWithMeleeCommand(Creature attacker, Creature target, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Target = target;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute()
        {
            var builder = new CombatAttackCreatureBuilder()
                .WithAttacker(Attacker)
                .WithTarget(Target)
                .WithProjectileType(null)
                .WithMagicEffectType(null)
                .WithMissedMagicEffectType(MagicEffectType.Puff)
                .WithDamageMagicEffectType(MagicEffectType.RedSpark)
                .WithAnimatedTextColor(AnimatedTextColor.DarkRed)
                .WithFormula(Formula);

            return builder.Build();
        }
    }
}