using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Strategies
{
    public interface IAttackStrategy
    {
        int CooldownInMilliseconds { get; }

        Command GetNext(Server server, Creature attacker, Creature target);
    }
}