using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaWithRuneCreateItemCommand : Command
    {
        public CombatAttackAreaWithRuneCreateItemCommand(Creature attacker, Position center, Offset[] area, ProjectileType projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            OpenTibiaId = openTibiaId;

            Count = count;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Position Center { get; set; }

        public Offset[] Area { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute()
        {
            var builder = new CombatAttackAreaBuilder()
                .WithAttacker(Attacker)
                .WithArea(Area, null)
                .WithCenter(Center)
                .WithProjectileType(ProjectileType)
                .WithMagicEffectType(MagicEffectType)
                .WithMissedMagicEffectType(null)
                .WithDamageMagicEffectType(null)
                .WithAnimatedTextColor(AnimatedTextColor.DarkRed)
                .WithFormula(Formula)
                .WithCreateItem(OpenTibiaId, Count);

            return builder.Build();
        }
    }
}