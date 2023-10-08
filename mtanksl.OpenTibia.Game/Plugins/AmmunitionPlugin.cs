using mtanksl.OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public abstract class AmmunitionPlugin : Plugin
    {
        public AmmunitionPlugin(Ammunition ammunition)
        {
            Ammunition = ammunition;
        }

        public Ammunition Ammunition { get; }
    }
}