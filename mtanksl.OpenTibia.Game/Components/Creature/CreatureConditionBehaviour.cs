using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class CreatureConditionBehaviour : Behaviour
    {
        public CreatureConditionBehaviour(Condition condition)
        {
            this.condition = condition;
        }

        private Condition condition;

        public Condition Condition
        {
            get
            {
                return condition;
            }
        }

        public override void Start()
        {
            //TODO
        }

        public override void Stop()
        {
            //TODO
        }
    }
}