using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Strategies
{
    public interface IWalkStrategy
    {
        Tile GetNext(Tile spawn, Creature creature, Creature target);
    }
}