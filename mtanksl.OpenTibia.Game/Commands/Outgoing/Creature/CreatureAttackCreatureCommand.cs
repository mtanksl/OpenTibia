using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class CreatureAttackCreatureCommand : Command
    {
        public CreatureAttackCreatureCommand(Creature attacker, Creature target, Attack attack)
        {
            Attacker = attacker;

            Target = target;

            Attack = attack;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public Attack Attack { get; set; }

        public override Promise Execute()
        {
            int damage = Attack.Calculate(Attacker, Target);

            if (damage == 0)
            {
                return Attack.Missed(Attacker, Target);
            }
             
            return Attack.Hit(Attacker, Target, damage);
        }
    }
}