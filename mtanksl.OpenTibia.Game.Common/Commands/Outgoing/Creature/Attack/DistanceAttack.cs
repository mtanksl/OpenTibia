using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class DistanceAttack : SimpleAttack
    {
        public DistanceAttack(ProjectileType projectileType, int min, int max) : base(projectileType, MagicEffectType.RedSpark, AnimatedTextColor.DarkRed, min, max)
        {

        }
    }
}