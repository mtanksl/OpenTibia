using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerSayPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnSay(Player player, string message);
    }
}