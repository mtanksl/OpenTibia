using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public abstract class ItemMoveScript : IScript
    {
        public abstract void Register(Server server);

        public abstract bool Execute(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, CommandContext context);
    }
}