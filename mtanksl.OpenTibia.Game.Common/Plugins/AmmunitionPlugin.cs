using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class AmmunitionPlugin : Plugin
    {
        public AmmunitionPlugin(Ammunition ammunition)
        {
            Ammunition = ammunition;
        }

        public Ammunition Ammunition { get; }

        public abstract PromiseResult<bool> OnUsingAmmunition(Player player, Creature target, Item weapon, Item ammunition);

        public abstract Promise OnUseAmmunition(Player player, Creature target, Item weapon, Item ammunition);        
    }
}