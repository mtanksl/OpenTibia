using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class Action
    {
        public abstract Promise Update(Creature attacker, Creature target);
    }
}