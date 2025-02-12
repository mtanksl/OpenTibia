using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class SnakebiteRodWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public SnakebiteRodWeaponPlugin(Weapon weapon) : base(weapon, 13, 5, DamageType.Earth)
        {

        }
    }
}