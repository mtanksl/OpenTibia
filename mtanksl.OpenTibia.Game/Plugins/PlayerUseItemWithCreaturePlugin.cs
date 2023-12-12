using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerUseItemWithCreaturePlugin : Plugin
    {
        public abstract PromiseResult<bool> OnUseItemWithCreature(Player player, Item item, Creature toCreature);
    }
}