using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaWithRuneAsBeamCommand : Command
    {
        public CombatAttackAreaWithRuneAsBeamCommand(Creature attacker, Position center, Offset[] area, ProjectileType projectileType, MagicEffectType magicEffectType, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Position Center { get; set; }

        public Offset[] Area { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute()
        {
            var builder = new CombatAttackAreaBuilder()
                .WithAttacker(Attacker)
                .WithArea(Area, Attacker.Direction)
                .WithCenter(Center)
                .WithProjectileType(ProjectileType)
                .WithMagicEffectType(MagicEffectType)
                .WithMissedMagicEffectType(null)
                .WithDamageMagicEffectType(null)
                .WithAnimatedTextColor(AnimatedTextColor.DarkRed)
                .WithFormula(Formula);

            return builder.Build();
        }
    }
}