using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface IItemUseWithCreatureScript : IScript
    {
        bool NextTo { get; }

        bool Execute(Player player, Item fromItem, Creature toCreature, Server server, CommandContext context);
    }
}