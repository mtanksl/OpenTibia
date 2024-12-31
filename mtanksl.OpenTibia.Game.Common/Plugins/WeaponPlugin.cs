using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class WeaponPlugin : Plugin
    {
        public WeaponPlugin(Weapon weapon)
        {
            Weapon = weapon;
        }

        public Weapon Weapon { get; }

        public abstract Promise OnUseWeapon(Player player, Creature target, Item weapon);
    }
}