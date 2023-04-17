using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CombatAddConditionCommand : Command
    {
        public CombatAddConditionCommand(Creature target, SpecialCondition specialCondition, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int[] damages, int intervalInMilliseconds)
        {
            Target = target;

            SpecialCondition = specialCondition;

            MagicEffectType = magicEffectType;

            AnimatedTextColor = animatedTextColor;

            Damages = damages;

            IntervalInMilliseconds = intervalInMilliseconds;
        }

        public Creature Target { get; set; }

        public SpecialCondition SpecialCondition { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int[] Damages { get; set; }

        public int IntervalInMilliseconds { get; set; }

        public override async Promise Execute()
        {
            if ( !Target.HasSpecialCondition(SpecialCondition) )
            {
                Target.AddSpecialCondition(SpecialCondition);

                if (Target is Player player)
                {
                    Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(Target.SpecialConditions) );
                }
            }

            for (int i = 0; i < Damages.Length; i++)
            {
                await Context.AddCommand(new CombatAddDamageCommand(null, Target, (attacker, target) => Damages[i], null, MagicEffectType, AnimatedTextColor) );

                if (Target.Health == 0)
                {
                    return;
                }

                if (i < Damages.Length - 1)
                {
                    await Context.Server.Components.AddComponent(Target, new CreatureSpecialConditionDelayBehaviour(SpecialCondition, IntervalInMilliseconds) ).Promise;
                }
            }

            {
                Target.RemoveSpecialCondition(SpecialCondition);

                if (Target is Player player)
                {
                    Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(Target.SpecialConditions));
                }
            }
        }
    }
}