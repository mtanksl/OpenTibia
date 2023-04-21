using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Strategies
{
    public interface IChooseTargetStrategy
    {
        Player GetNext(Server server, Creature attacker);
    }
}