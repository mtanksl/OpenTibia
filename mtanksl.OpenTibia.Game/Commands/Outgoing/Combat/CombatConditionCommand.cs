using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CombatConditionCommand : Command
    {
        public CombatConditionCommand(Creature target, SpecialCondition specialCondition, MagicEffectType? magicEffectType, int[] health, int[] cooldownInMilliseconds)
        {
            SpecialCondition = specialCondition;

            Target = target;

            MagicEffectType = magicEffectType;

            Health = health;

            CooldownInMilliseconds = cooldownInMilliseconds;
        }

        public Creature Target { get; set; }
        
        public SpecialCondition SpecialCondition { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public int[] Health { get; set; }

        public int[] CooldownInMilliseconds { get; set; }

        private int index;

        public override Promise Execute(Context context)
        {
            if (index < Health.Length - 1)
            {
                SpecialConditionBehaviour component = context.Server.Components.GetComponent<SpecialConditionBehaviour>(Target);

                if (component != null)
                {
                    if ( !component.HasSpecialCondition(SpecialCondition) )
                    {
                        component.AddSpecialCondition(SpecialCondition);

                        if (Target is Player player)
                        {
                            context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(component.SpecialConditions) );
                        }
                    }
                }
                               
                if (MagicEffectType != null)
                {
                    context.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MagicEffectType.Value) );
                }

                context.AddCommand(new CombatChangeHealthCommand(null, Target, MagicEffectType.ToAnimatedTextColor(), Health[index] ) );

                if (Target.Tile != null)
                {
                    context.Server.Components.AddComponent(Target, new DelayBehaviour("Combat_Condition_" + SpecialCondition, CooldownInMilliseconds[index] ) ).Promise.Then(ctx =>
                    {
                        index++;

                        return Execute(ctx);
                    } );
                }
            }
            else
            {
                SpecialConditionBehaviour component = context.Server.Components.GetComponent<SpecialConditionBehaviour>(Target);

                if (component != null)
                {
                    component.RemoveSpecialCondition(SpecialCondition);

                    if (Target is Player player)
                    {
                        context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(component.SpecialConditions) );
                    }
                }

                if (MagicEffectType != null)
                {
                    context.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MagicEffectType.Value) );
                }

                context.AddCommand(new CombatChangeHealthCommand(null, Target, MagicEffectType.ToAnimatedTextColor(), Health[index] ) );
            }    
            
            return Promise.FromResult(context);
        }
    }
}