using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Strategies
{
    public class MeleeAttackStrategy : IAttackStrategy
    {
        private int cooldownInMilliseconds;

        public MeleeAttackStrategy(int cooldownInMilliseconds)
        {
            this.cooldownInMilliseconds = cooldownInMilliseconds;
        }

        public int CooldownInMilliseconds
        {
            get
            {
                return cooldownInMilliseconds;
            }
        }

        public Command GetNext(Server server, Creature attacker, Creature target)
        {
            if (attacker.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return new CreatureAttackCreatureCommand(attacker, target, new MeleeAttack() );
            }

            return null;
        }
    }
}