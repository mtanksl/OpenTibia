using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class MonsterThinkBehaviour : Behaviour
    {
        private IChooseTargetStrategy chooseTargetStrategy;

        private BehaviourAction[] actions;

        public MonsterThinkBehaviour(IChooseTargetStrategy chooseTargetStrategy, BehaviourAction[] actions)
        {
            this.chooseTargetStrategy = chooseTargetStrategy;

            this.actions = actions;
        }

        private Guid globalTick;

        public override void Start(Server server)
        {
            Creature attacker = (Creature)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>(async (context, e) =>
            {
                Creature target = chooseTargetStrategy.GetNext(Context.Server, attacker);

                if (target != null)
                {
                    foreach (var action in actions)
                    {
                        await action.Update(attacker, target);
                    }
                }
            } );
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}