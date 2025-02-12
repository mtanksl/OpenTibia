using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class QuagmireRodWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public QuagmireRodWeaponPlugin(Weapon weapon) : base(weapon, 45, 8, DamageType.Earth)
        {

        }
    }
}