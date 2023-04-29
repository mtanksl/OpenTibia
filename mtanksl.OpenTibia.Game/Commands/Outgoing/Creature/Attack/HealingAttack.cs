using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class HealingAttack : Attack
    {
        public HealingAttack(MagicEffectType? magicEffectType, int damage)
        {
            MagicEffectType = magicEffectType;

            Damage = damage;
        }

        public MagicEffectType? MagicEffectType { get; set; }

        public int Damage { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            return Damage;
        }

        public override Promise Missed(Creature attacker, Creature target)
        {
            return Promise.Completed;
        }

        public override Promise Hit(Creature attacker, Creature target, int damage)
        {
            return Promise.Run( () =>
            {
                if (MagicEffectType != null)
                {
                     return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.Value) );
                }

                return Promise.Completed;

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Poisoned) );

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Slowed) );
            } );
        }
    }
}