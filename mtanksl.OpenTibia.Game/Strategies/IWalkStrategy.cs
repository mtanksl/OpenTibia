using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Strategies
{
    public interface IWalkStrategy
    {
        Tile GetNext(Server server, Tile spawn, Creature attacker, Creature target);
    }
}