using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface IItemUseScript : IScript
    {
        bool OnItemUse(Player player, Item item, Server server, Context context);
    }
}