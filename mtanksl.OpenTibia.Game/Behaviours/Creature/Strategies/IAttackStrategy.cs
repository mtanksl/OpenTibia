using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public interface IAttackStrategy
    {
        Command GetNext(Server server, Creature attacker, Creature target);
    }
}