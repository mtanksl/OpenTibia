using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerUseItemWithItemPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnUseItemWithItem(Player player, Item item, Item toItem);
    }
}