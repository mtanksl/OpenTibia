using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class CombatConditionCommand : Command
    {
        public CombatConditionCommand(Creature target, MagicEffectType magicEffectType, int[] health, int[] cooldownInMilliseconds)
        {
            Target = target;

            MagicEffectType = magicEffectType;

            Health = health;

            CooldownInMilliseconds = cooldownInMilliseconds;
        }

        public Creature Target { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public int[] Health { get; set; }

        public int[] CooldownInMilliseconds { get; set; }

        private int index;

        public override Promise Execute(Context context)
        {
            if (index < Health.Length && Target.Tile != null)
            {
                return context.AddCommand(CombatCommand.TargetAttack(null, Target, null, MagicEffectType, (attacker, target) => Health[index] ) ).Then(ctx =>
                {
                    return ctx.Server.Components.AddComponent(Target, new DecayBehaviour(CooldownInMilliseconds[index++] ) ).Promise;

                } ).Then(ctx =>
                {
                    return Execute(ctx);
                } );
            }

            return Promise.FromResult(context);
        }
    }
}