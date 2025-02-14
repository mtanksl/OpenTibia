using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public abstract class Attack
    {
        public abstract (int Damage, BlockType BlockType) Calculate(Creature attacker, Creature target);

        public abstract Promise NoDamage(Creature attacker, Creature target, BlockType blockType);

        public abstract Promise Damage(Creature attacker, Creature target, int damage);
    }    
}