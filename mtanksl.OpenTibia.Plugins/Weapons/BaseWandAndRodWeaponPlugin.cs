using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public abstract class BaseWandAndRodWeaponPlugin : WeaponPlugin
    {
        private int attackStrength;
        
        private int attackVariation;

        private DamageType damageType;

        public BaseWandAndRodWeaponPlugin(Weapon weapon, int attackStrength, int attackVariation, DamageType damageType) : base(weapon)
        {
            this.attackStrength = attackStrength;

            this.attackVariation = attackVariation;

            this.damageType = damageType;
        }

        public override PromiseResult<bool> OnUsingWeapon(Player player, Creature target, Item weapon)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
            var formula = Formula.WandFormula(attackStrength, attackVariation);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DamageAttack(weapon.Metadata.ProjectileType.Value, null, damageType, formula.Min, formula.Max, true) ) );
        }
    }
}