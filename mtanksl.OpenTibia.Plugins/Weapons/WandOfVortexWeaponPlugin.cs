using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class WandOfVortexWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public WandOfVortexWeaponPlugin(Weapon weapon) : base(weapon, 13, 5, DamageType.Energy)
        {

        }
    }
}