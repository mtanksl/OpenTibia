using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerRotateItemPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnRotateItem(Player player, Item item);
    }
}