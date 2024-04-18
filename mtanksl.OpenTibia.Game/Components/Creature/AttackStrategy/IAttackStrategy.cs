using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public interface IAttackStrategy
    {
        bool CanAttack(Creature attacker, Creature target);

        Promise Attack(Creature attacker, Creature target);

        TimeSpan Cooldown { get; }
    }
}