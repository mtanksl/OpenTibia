using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class CreatureAction
    {
        public abstract Promise Update(Creature attacker, Creature target);
    }
}