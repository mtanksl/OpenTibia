using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public interface ITargetStrategy
    {
        Player GetTarget(Creature attacker, Player[] visiblePlayers);
    }
}