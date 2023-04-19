using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaWithRuneAsRadialCommand : Command
    {
        public CombatAttackAreaWithRuneAsRadialCommand(Creature attacker, Position center, Offset[] area, ProjectileType projectileType, MagicEffectType magicEffectType, Func<Creature, Creature, int> formula)
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
            CombatAttackAreaBuilder builder = new CombatAttackAreaBuilder()
            {
                Attacker = Attacker,
                Center = Center,
                Area = Area,
                Direction = null,
                ProjectileType = ProjectileType,
                MagicEffectType = MagicEffectType,
                OpenTibiaId = null,
                Count = null,
                Formula = new DamageDto()
                {
                    Formula = Formula,
                    MissedMagicEffectType = null,
                    DamageMagicEffectType = null,
                    DamageAnimatedTextColor = AnimatedTextColor.DarkRed
                },
                Condition = null
            };

            return builder.Build();
        }
    }
}