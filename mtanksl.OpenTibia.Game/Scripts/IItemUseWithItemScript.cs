using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface IItemUseWithItemScript : IScript
    {
        bool NextTo { get; }
        
        bool Execute(Player player, Item fromItem, Item toItem, Server server, CommandContext context);
    }
}