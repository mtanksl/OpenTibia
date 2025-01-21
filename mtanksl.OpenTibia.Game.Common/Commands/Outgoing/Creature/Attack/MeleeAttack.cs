using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MeleeAttack : SimpleAttack
    {
        public MeleeAttack(int min, int max) : base(null, MagicEffectType.RedSpark, DamageType.Physical, min, max)
        {

        } 
    }
}