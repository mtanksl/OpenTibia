using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public interface IWalkStrategy
    {
        Tile GetNext(Server server, Tile spawn, Creature attacker, Creature target);
    }
}