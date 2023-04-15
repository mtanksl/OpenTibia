using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Components
{
    public class CreatureSpecialConditionBehaviour : Behaviour
    {
        private Creature creature;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;
        }

        private SpecialCondition specialConditions;

        public SpecialCondition SpecialConditions
        {
            get
            {
                return specialConditions;
            }
        }

        public bool HasSpecialCondition(SpecialCondition specialCondition)
        {
            return (specialConditions & specialCondition) == specialCondition;
        }

        public void AddSpecialCondition(SpecialCondition specialCondition)
        {
            specialConditions |= specialCondition;


        }

        public void RemoveSpecialCondition(SpecialCondition specialCondition)
        {
            specialConditions &= ~specialCondition;


        }

        public override void Stop(Server server)
        {
            
        }
    }
}