using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaWithSpellAsBeamCommand : Command
    {
        public CombatAttackAreaWithSpellAsBeamCommand(Creature attacker, Offset[] area, MagicEffectType magicEffectType, Func<Creature, Creature, int> formula)
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
            var builder = new CombatAttackAreaBuilder()
                .WithAttacker(Attacker)
                .WithArea(Area, Attacker.Direction)
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