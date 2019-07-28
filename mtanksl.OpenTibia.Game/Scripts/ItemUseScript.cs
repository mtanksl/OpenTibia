using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public abstract class ItemUseScript : IScript
    {
        public abstract void Register(Server server);

        public abstract bool Execute(Player player, Item item, Server server, CommandContext context);
    }
}