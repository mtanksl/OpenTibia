using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class DistanceAttack : SimpleAttack
    {
        public DistanceAttack(ProjectileType projectileType, int min, int max) : base(projectileType, MagicEffectType.RedSpark, AnimatedTextColor.DarkRed, min, max)
        {

        }

        public override async Promise Missed(Creature attacker, Creature target)
        {
            if (ShowProjectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker, target, ShowProjectileType.Value) );
            }
            
            await base.Missed(attacker, target);
        }
    }
}