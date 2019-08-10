using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Scripts
{
    public interface ICreatureWalkScript : IScript
    {
        bool OnCreatureWalk(Creature creature, Tile fromTile, Tile toTile, Server server, Context context);
    }
}