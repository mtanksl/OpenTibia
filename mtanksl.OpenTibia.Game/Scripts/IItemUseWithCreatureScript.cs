using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface IItemUseWithCreatureScript : IScript
    {
        bool NextTo { get; }

        bool OnItemUseWithCreature(Player player, Item item, Creature toCreature, Server server, Context context);
    }
}