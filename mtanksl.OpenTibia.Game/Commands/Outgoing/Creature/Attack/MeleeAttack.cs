using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MeleeAttack : Attack
    {
       public MeleeAttack(int? min, int? max)
        {
            Min = min;

            Max = max;
        }

        public int? Min { get; set; }

        public int? Max { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            if (Min == null || Max == null)
            {
                //TODO: Calculate melee damage

                return -Context.Current.Server.Randomization.Take(0, 30);
            }

            return -Context.Current.Server.Randomization.Take(Min.Value, Max.Value);
        }

        public override Promise Missed(Creature attacker, Creature target)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.Puff) );
        }

        public override Promise Hit(Creature attacker, Creature target, int damage)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.RedSpark) ).Then( () =>
            {
                return Context.Current.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, AnimatedTextColor.DarkRed, (-damage).ToString() ) );

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );
            } );
        }       
    }
}