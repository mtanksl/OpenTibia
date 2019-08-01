using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface IItemMoveScript : IScript
    {
        bool OnItemMove(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, CommandContext context);
    }
}