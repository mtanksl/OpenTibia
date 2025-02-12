using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class WandOfDragonbreathWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public WandOfDragonbreathWeaponPlugin(Weapon weapon) : base(weapon, 19, 6, DamageType.Fire)
        {

        }
    }
}