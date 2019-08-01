using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface ITileAddCreatureScript : IScript
    {
        void OnTileAddCreature(Creature creature, Tile toTile, byte toIndex, Server server, CommandContext context);
    }
}