using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaWithRuneCreateItemCommand : Command
    {
        public CombatAttackAreaWithRuneCreateItemCommand(Creature attacker, Position center, Offset[] area, ProjectileType projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, ConditionDto condition)
        {
            Attacker = attacker;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            OpenTibiaId = openTibiaId;

            Count = count;

            Condition = condition;
        }

        public Creature Attacker { get; set; }

        public Position Center { get; set; }

        public Offset[] Area { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public ConditionDto Condition { get; set; }

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
                OpenTibiaId = OpenTibiaId,
                Count = Count,
                Formula = null,
                Condition = Condition
            };

            return builder.Build();
        }
    }
}