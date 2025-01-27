using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public abstract class Attack
    {
        protected Attack(DamageType damageType, int min, int max)
        {
            DamageType = damageType;

            Min = min;

            Max = max;
        }

        public DamageType DamageType { get; }

        public int Min { get; }

        public int Max { get; }

        public virtual int Calculate(Creature attacker, Creature target)
        {
            if (target is Monster monster)
            {
                // pvm or mvm

                double elementPercent;

                if ( !monster.Metadata.DamageTakenFromElements.TryGetValue(DamageType, out elementPercent) )
                {
                    elementPercent = 1;                   
                }

                return (int)(Context.Current.Server.Randomization.Take(Min, Max) * elementPercent);
            }

            if (target is Player player)
            {
                double attackPercent;

                int shielding;

                if (attacker == null)
                {
                    // environment

                    attackPercent = 1;

                    shielding = 0;
                }
                else
                {
                    if (attacker is Monster)
                    {
                        // mvp

                        attackPercent = 1;
                    }
                    else
                    {
                        // pvp

                        if (player.Combat.GetSkullIcon(null) == SkullIcon.Black)
                        {
                            attackPercent = 1;
                        }
                        else
                        {
                            attackPercent = 0.5;
                        }
                    }

                    shielding = Formula.ShieldingFormula(player.Inventory.GetDefense(), player.Inventory.GetArmor(), player.Client.FightMode);
                }

                int damage = (int)(Context.Current.Server.Randomization.Take(Min, Max) * attackPercent) - shielding;

                if (damage > 0)
                {
                    return damage;
                }
            }

            return 0;
        }

        public abstract Promise Missed(Creature attacker, Creature target);

        public abstract Promise Hit(Creature attacker, Creature target, int damage);
    }    
}