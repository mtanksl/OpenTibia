using mtanksl.OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public abstract class WeaponPlugin : Plugin
    {
        public WeaponPlugin(Weapon weapon)
        {
            Weapon = weapon;
        }

        public Weapon Weapon { get; }
    }
}