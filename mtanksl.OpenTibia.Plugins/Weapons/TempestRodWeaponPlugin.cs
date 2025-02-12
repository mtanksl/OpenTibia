using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class TempestRodWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public TempestRodWeaponPlugin(Weapon weapon) : base(weapon, 65, 9, DamageType.Energy)
        {

        }
    }
}