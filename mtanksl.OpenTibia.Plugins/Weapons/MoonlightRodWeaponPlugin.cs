using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class MoonlightRodWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public MoonlightRodWeaponPlugin(Weapon weapon) : base(weapon, 19, 6, DamageType.Ice)
        {

        }
    }
}