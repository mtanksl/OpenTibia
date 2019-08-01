using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface ITileRemoveCreatureScript : IScript
    {
        void OnTileRemoveCreature(Creature creature, Tile fromTile, byte fromIndex, Server server, CommandContext context);
    }
}