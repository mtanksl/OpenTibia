using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerUseItemPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnUseItem(Player player, Item item);
    }
}