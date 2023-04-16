using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Components
{
    public class CreatureSpecialConditionDelayBehaviour : DelayBehaviour
    {
        public CreatureSpecialConditionDelayBehaviour(SpecialCondition specialCondition, int executeInMilliseconds) : base("CreatureSpecialConditionDelayBehaviour" + specialCondition, executeInMilliseconds)
        {
            this.specialCondition = specialCondition;
        }

        private SpecialCondition specialCondition;

        public SpecialCondition SpecialCondition
        {
            get
            {
                return specialCondition;
            }
        }

        public override void Start(Server server)
        {
            base.Start(server);
        }

        public override void Stop(Server server)
        {
            base.Stop(server);
        }
    }
}