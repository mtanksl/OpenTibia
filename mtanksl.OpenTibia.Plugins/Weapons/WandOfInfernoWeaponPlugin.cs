using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Weapons
{
    public class WandOfInfernoWeaponPlugin : BaseWandAndRodWeaponPlugin
    {
        public WandOfInfernoWeaponPlugin(Weapon weapon) : base(weapon, 65, 9, DamageType.Fire)
        {

        }
    }
}