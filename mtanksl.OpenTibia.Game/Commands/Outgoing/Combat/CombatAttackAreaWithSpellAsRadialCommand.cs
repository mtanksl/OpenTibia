using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaWithSpellAsRadialCommand : Command
    {
        public CombatAttackAreaWithSpellAsRadialCommand(Creature attacker, Offset[] area, MagicEffectType magicEffectType, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Area = area;

            MagicEffectType = magicEffectType;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Offset[] Area { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute()
        {
            CombatAttackAreaBuilder builder = new CombatAttackAreaBuilder()
                .WithAttacker(Attacker)
                .WithArea(Area, null)
                .WithCenter(Attacker.Tile.Position)
                .WithProjectileType(null)
                .WithMagicEffectType(MagicEffectType)
                .WithMissedMagicEffectType(null)
                .WithDamageMagicEffectType(null)
                .WithAnimatedTextColor(AnimatedTextColor.DarkRed)
                .WithFormula(Formula);

            return builder.Build();
        }
    }
}