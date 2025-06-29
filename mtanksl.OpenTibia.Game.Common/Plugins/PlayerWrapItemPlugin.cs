using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerWrapItemPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnWrapItem(Player player, Item item);
    }
}