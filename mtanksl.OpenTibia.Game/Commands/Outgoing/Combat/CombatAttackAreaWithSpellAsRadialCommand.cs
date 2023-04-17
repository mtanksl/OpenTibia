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
            {
                Attacker = Attacker,
                Center = Attacker.Tile.Position,
                Area = Area,
                Direction = null,
                ProjectileType = null,
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