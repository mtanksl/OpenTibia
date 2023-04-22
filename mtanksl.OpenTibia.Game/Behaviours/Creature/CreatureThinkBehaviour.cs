using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureThinkBehaviour : Behaviour
    {
        private IChooseTargetStrategy chooseTargetStrategy;

        private Action[] actions;

        public CreatureThinkBehaviour(IChooseTargetStrategy chooseTargetStrategy, Action[] actions)
        {
            this.chooseTargetStrategy = chooseTargetStrategy;

            this.actions = actions;
        }

        public override bool IsUnique
        {
            get
            {
                return true;
            }
        }

        private Creature attacker;

        private Guid token;

        public override void Start(Server server)
        {
            attacker = (Creature)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                return Update();
            } );
        }

        private async Promise Update()
        {
            Creature target = chooseTargetStrategy.GetNext(Context.Server, attacker);

            if (target != null)
            {
                foreach (var action in actions)
                {
                    await action.Update(attacker, target);
                }
            }
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(token);
        }
    }
}