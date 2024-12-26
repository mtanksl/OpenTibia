using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class SpellAreaAttackStrategy : IAttackStrategy
    {
        private Offset[] area;

        private MagicEffectType? magicEffectType;

        private AnimatedTextColor? animatedTextColor;

        private int min;

        private int max;

        public SpellAreaAttackStrategy(Offset[] area, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int min, int max)
        {
            this.area = area;

            this.magicEffectType = magicEffectType;

            this.animatedTextColor = animatedTextColor;

            this.min = min;

            this.max = max;
        }

        public bool CanAttack(Creature attacker, Creature target)
        {
            return true;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, magicEffectType, 
                        
                new SimpleAttack(null, null, animatedTextColor, min, max) ) );
        }
    }
}