using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class MeleeAttackStrategy : IAttackStrategy
    {
        private int? min;

        private int? max;

        public MeleeAttackStrategy(int? min, int? max)
        {
            this.min = min;

            this.max = max;
        }

        public Command GetNext(Server server, Creature attacker, Creature target)
        {
            if (attacker.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return new CreatureAttackCreatureCommand(attacker, target, new MeleeAttack(min, max) );
            }

            return null;
        }
    }
}