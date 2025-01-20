using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public interface IAttackStrategy
    {
        PromiseResult<bool> CanAttack(Creature attacker, Creature target);

        Promise Attack(Creature attacker, Creature target);
    }
}