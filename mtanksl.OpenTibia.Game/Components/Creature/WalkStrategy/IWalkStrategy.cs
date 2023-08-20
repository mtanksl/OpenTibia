using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public interface IWalkStrategy
    {
        bool CanWalk(Creature attacker, Creature target, out Tile tile);
    }
}