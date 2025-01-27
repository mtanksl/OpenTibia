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
            // pvm or mvm

            if (target is Monster monster)
            {
                double elementPercent;

                if (monster.Metadata.DamageTakenFromElements.TryGetValue(DamageType, out elementPercent) )
                {
                    return (int)(Context.Current.Server.Randomization.Take(Min, Max) * elementPercent);
                }

                return Context.Current.Server.Randomization.Take(Min, Max);
            }

            // environment

            if (attacker == null)
            {
                return Context.Current.Server.Randomization.Take(Min, Max);
            }

            // mvp

            if (attacker is Monster)
            {
                return Context.Current.Server.Randomization.Take(Min, Max);
            }

            // pvp

            if ( ( (Player)target).Combat.GetSkullIcon(null) == SkullIcon.Black)
            {
                return Context.Current.Server.Randomization.Take(Min, Max);
            }

            return (int)(Context.Current.Server.Randomization.Take(Min, Max) / 2.0);
        }

        public abstract Promise Missed(Creature attacker, Creature target);

        public abstract Promise Hit(Creature attacker, Creature target, int damage);
    }    
}