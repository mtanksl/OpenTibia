using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public abstract class ItemUseWithItemScript : IScript
    {
        public abstract void Register(Server server);

        public abstract bool NextTo { get; }

        public abstract bool Execute(Player player, Item fromItem, Item toItem, Server server, CommandContext context);
    }
}