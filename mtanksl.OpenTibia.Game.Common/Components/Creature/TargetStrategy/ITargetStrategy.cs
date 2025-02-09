using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public interface ITargetStrategy
    {
        Player GetTarget(int ticks, Creature attacker, Player[] players);
    }
}