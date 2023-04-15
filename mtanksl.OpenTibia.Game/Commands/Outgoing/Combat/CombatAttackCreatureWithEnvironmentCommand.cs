using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackCreatureWithEnvironmentCommand : Command
    {
        public CombatAttackCreatureWithEnvironmentCommand(Creature target, MagicEffectType damageMagicEffectType, int damage)
        {
            Target = target;

            DamageMagicEffectType = damageMagicEffectType;

            Damage = damage;
        }

        public Creature Target { get; set; }

        public MagicEffectType DamageMagicEffectType { get; set; }

        public int Damage { get; set; }

        public override Promise Execute()
        {
            CombatAttackCreatureBuilder builder = new CombatAttackCreatureBuilder()
                .WithAttacker(null)
                .WithTarget(Target)
                .WithProjectileType(null)
                .WithMagicEffectType(null)
                .WithMissedMagicEffectType(null)
                .WithDamageMagicEffectType(DamageMagicEffectType)
                .WithAnimatedTextColor(AnimatedTextColor.DarkRed)
                .WithFormula( (attacker, target) => Damage );

            return builder.Build();
        }
    }
}