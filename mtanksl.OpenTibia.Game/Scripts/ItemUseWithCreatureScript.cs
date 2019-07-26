using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public abstract class ItemUseWithCreatureScript : IScript
    {
        public abstract void Register(Server server);

        public abstract bool Execute(Player player, Item fromItem, Creature toCreature, Server server, CommandContext context);
    }
}