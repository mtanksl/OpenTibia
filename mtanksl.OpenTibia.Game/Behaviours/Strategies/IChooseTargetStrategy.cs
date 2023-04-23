using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public interface IChooseTargetStrategy
    {
        Player GetNext(Server server, Creature attacker);
    }
}