using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class WandOfCosmicEnergyWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public WandOfCosmicEnergyWeaponPlugin(Weapon weapon) : base(weapon, 45, 8, DamageType.Energy)
        {

        }
    }
}