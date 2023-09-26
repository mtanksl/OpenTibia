using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class MeleeAttackStrategy : IAttackStrategy
    {
        private int min;

        private int max;

        private TimeSpan cooldown;

        public MeleeAttackStrategy(int min, int max, TimeSpan cooldown)
        {
            this.min = min;

            this.max = max;

            this.cooldown = cooldown;
        }

        public TimeSpan Cooldown
        {
            get
            {
                return cooldown;
            }
        }

        public bool CanAttack(Creature attacker, Creature target)
        {
            if (attacker.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, new MeleeAttack(min, max) ) );            
        }
    }
}