using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class DamageCondition : Condition
    {
        private DelayBehaviour delayBehaviour;

        public DamageCondition(SpecialCondition specialCondition, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int[] damages, int intervalInMilliseconds) : base( (ConditionSpecialCondition)specialCondition)
        {
            SpecialCondition = specialCondition;

            MagicEffectType = magicEffectType;

            AnimatedTextColor = animatedTextColor;

            Damages = damages;

            IntervalInMilliseconds = intervalInMilliseconds;
        }

        public SpecialCondition SpecialCondition { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int[] Damages { get; set; }

        public int IntervalInMilliseconds { get; set; }

        public override async Promise Update(Creature target)
        {
            for (int i = 0; i < Damages.Length; i++)
            {
                await Context.Current.AddCommand(new CreatureAttackCreatureCommand(null, target, new SimpleAttack(null, MagicEffectType, AnimatedTextColor, Damages[i] ) ) );

                if (i < Damages.Length - 1)
                {
                    delayBehaviour = Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(IntervalInMilliseconds) );

                    await delayBehaviour.Promise;
                }
            }
        }

        public override void Stop(Server server)
        {
            if (delayBehaviour != null)
            {
                delayBehaviour.Stop(server);
            }
        }
    }
}