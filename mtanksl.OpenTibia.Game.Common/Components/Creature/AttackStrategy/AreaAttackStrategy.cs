using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class AreaAttackStrategy : IAttackStrategy
    {
        private Offset[] area;

        private ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        private AnimatedTextColor? animatedTextColor;

        private int min;

        private int max;

        public AreaAttackStrategy(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int min, int max)
        {
            this.area = area;

            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.animatedTextColor = animatedTextColor;

            this.min = min;

            this.max = max;
        }

        public bool CanAttack(Creature attacker, Creature target)
        {
            if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public Promise Attack(Creature attacker, Creature target)
        {            
            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, target.Tile.Position, area, projectileType, magicEffectType, 
                
                new SimpleAttack(null, null, animatedTextColor, min, max) ) );
        }
    }
}