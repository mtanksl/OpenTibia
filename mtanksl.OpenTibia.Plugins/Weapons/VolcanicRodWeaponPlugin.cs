using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class VolcanicRodWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public VolcanicRodWeaponPlugin(Weapon weapon) : base(weapon, 30, 7, DamageType.Fire)
        {

        }
    }
}