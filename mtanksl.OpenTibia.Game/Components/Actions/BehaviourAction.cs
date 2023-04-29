using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class BehaviourAction
    {
        public abstract Promise Update(Creature attacker, Creature target);
    }
}