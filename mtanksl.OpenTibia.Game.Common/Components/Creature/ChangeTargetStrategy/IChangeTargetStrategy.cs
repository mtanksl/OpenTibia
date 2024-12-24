using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public interface IChangeTargetStrategy
    {
        bool ShouldChange(Creature attacker, Player target);
    }
}