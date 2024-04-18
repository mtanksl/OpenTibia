using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public abstract class Attack
    {
        public abstract int Calculate(Creature attacker, Creature target);

        public abstract Promise Missed(Creature attacker, Creature target);

        public abstract Promise Hit(Creature attacker, Creature target, int damage);
    }    
}