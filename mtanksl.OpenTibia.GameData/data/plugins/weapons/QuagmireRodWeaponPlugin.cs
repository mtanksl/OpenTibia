using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.GameData.Plugins.Weapons
{
    public class QuagmireRodWeaponPlugin : WeaponPlugin
    {
        public QuagmireRodWeaponPlugin(Weapon weapon) : base(weapon)
        {

        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
           var formula = WandFormula(45, 8);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DistanceAttack(weapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
        }
    }
}