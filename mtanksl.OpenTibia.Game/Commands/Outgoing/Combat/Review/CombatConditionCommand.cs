using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CombatConditionCommand : Command
    {
        public CombatConditionCommand(Creature attacker, Creature target, SpecialCondition specialCondition, MagicEffectType? magicEffectType, int[] health, int cooldownInMilliseconds)
        {
            Attacker = attacker;

            Target = target;

            SpecialCondition = specialCondition;

            MagicEffectType = magicEffectType;

            Health = health;

            CooldownInMilliseconds = cooldownInMilliseconds;
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }
        
        public SpecialCondition SpecialCondition { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public int[] Health { get; set; }

        public int CooldownInMilliseconds { get; set; }

        private int index;

        public override Promise Execute()
        {
            if (MagicEffectType != null)
            {
                Context.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MagicEffectType.Value) );
            }

            Context.AddCommand(new CombatChangeHealthCommand(Attacker, Target, MagicEffectType.ToAnimatedTextColor(), Health[index] ) );

            if (Target.Tile != null)
            {
                CreatureSpecialConditionBehaviour playerAttackAndFollowBehaviour = Context.Server.Components.GetComponent<CreatureSpecialConditionBehaviour>(Target);

                if (playerAttackAndFollowBehaviour != null)
                {
                    if (index < Health.Length - 1)
                    {
                        if ( !playerAttackAndFollowBehaviour.HasSpecialCondition(SpecialCondition) )
                        {
                            playerAttackAndFollowBehaviour.AddSpecialCondition(SpecialCondition);

                            if (Target is Player player)
                            {
                                Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(playerAttackAndFollowBehaviour.SpecialConditions) );
                            }
                        }

                        Context.Server.Components.AddComponent(Target, new DelayBehaviour("Combat_Condition_" + SpecialCondition, CooldownInMilliseconds) ).Promise.Then( () =>
                        {
                            index++;

                            return Execute();
                        } );
                    }
                    else
                    {
                        if (playerAttackAndFollowBehaviour.HasSpecialCondition(SpecialCondition) )
                        {
                            playerAttackAndFollowBehaviour.RemoveSpecialCondition(SpecialCondition);

                            if (Target is Player player)
                            {
                                Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(playerAttackAndFollowBehaviour.SpecialConditions) );
                            }
                        }

                        Context.Server.CancelQueueForExecution("Combat_Condition_" + SpecialCondition + Target.Id);
                    }
                }
            }

            return Promise.Completed;
        }
    }
}