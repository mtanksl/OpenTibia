using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Components
{
    public abstract class CreatureConditionBehaviour : Behaviour
    {
        public CreatureConditionBehaviour(ConditionSpecialCondition conditionSpecialCondition)
        {
            ConditionSpecialCondition = conditionSpecialCondition;
        }

        public ConditionSpecialCondition ConditionSpecialCondition { get; set; }

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            SpecialCondition specialCondition = ConditionSpecialCondition.ToSpecialCondition();

            if (specialCondition != SpecialCondition.None && !creature.HasSpecialCondition(specialCondition) )
            {
                creature.AddSpecialCondition(specialCondition);

                if (creature is Player player)
                {
                    Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(creature.SpecialConditions) );
                }
            }
        }

        public override void Stop()
        {
            Creature creature = (Creature)GameObject;

            SpecialCondition specialCondition = ConditionSpecialCondition.ToSpecialCondition();

            if (specialCondition != SpecialCondition.None && creature.HasSpecialCondition(specialCondition) )
            {
                creature.RemoveSpecialCondition(specialCondition);

                if (creature is Player player)
                {
                    Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(creature.SpecialConditions) );
                }
            }
        }
    }
}