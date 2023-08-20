using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class SimpleAttack : Attack
    {
        public SimpleAttack(ProjectileType? projectileType, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int min, int max)
        {
            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            AnimatedTextColor = animatedTextColor;

            Min = min;

            Max = max;
        }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            return -Context.Current.Server.Randomization.Take(Min, Max);
        }

        public override Promise Missed(Creature attacker, Creature target)
        {
            return Promise.Completed;
        }

        public override Promise Hit(Creature attacker, Creature target, int damage)
        {
            return Promise.Run( () =>
            {
                if (ProjectileType != null)
                {
                    return Context.Current.AddCommand(new ShowProjectileCommand(attacker.Tile.Position, target.Tile.Position, ProjectileType.Value) );
                }

                return Promise.Completed;

            } ).Then( () =>
            {
                if (MagicEffectType != null)
                {
                     return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.Value) );
                }

                return Promise.Completed;

            } ).Then( () =>
            {
                if (AnimatedTextColor != null)
                {
                    if (attacker != target)
                    {
                        return Context.Current.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, AnimatedTextColor.Value, (-damage).ToString() ) );
                    }
                }

                return Promise.Completed;

            } ).Then( () =>
            {
                if (attacker != target)
                {
                    return Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );
                }

                return Promise.Completed;
            } );
        }
    }
}