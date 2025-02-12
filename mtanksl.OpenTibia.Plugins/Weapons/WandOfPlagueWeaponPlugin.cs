using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class WandOfPlagueWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public WandOfPlagueWeaponPlugin(Weapon weapon) : base(weapon, 30, 7, DamageType.Earth)
        {

        }
    }
}