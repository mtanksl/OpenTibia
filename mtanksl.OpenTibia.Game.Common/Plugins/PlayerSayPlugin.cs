using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerSayPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnSay(Player player, string message);
    }
}