using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public interface IChangeTargetStrategy
    {
        bool ShouldChange(int ticks, Creature attacker, Creature target);
    }
}