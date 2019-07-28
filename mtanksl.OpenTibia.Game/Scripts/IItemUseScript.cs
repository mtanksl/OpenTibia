using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface IItemUseScript : IScript
    {
        bool Execute(Player player, Item item, Server server, CommandContext context);
    }
}