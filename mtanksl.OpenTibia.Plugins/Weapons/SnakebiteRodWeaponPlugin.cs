using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class SnakebiteRodWeaponPlugin : WeaponPlugin
    {
        public SnakebiteRodWeaponPlugin(Weapon weapon) : base(weapon)
        {

        }

        public override PromiseResult<bool> OnUsingWeapon(Player player, Creature target, Item weapon)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
           var formula = Formula.WandFormula(13, 5);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DamageAttack(weapon.Metadata.ProjectileType.Value, null, DamageType.Earth, formula.Min, formula.Max) ) );
        }
    }
}