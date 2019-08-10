using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Scripts
{
    public interface IItemMoveScript : IScript
    {
        bool OnItemMove(Player player, Item item, IContainer toContainer, byte toIndex, byte count, Server server, Context context);
    }
}