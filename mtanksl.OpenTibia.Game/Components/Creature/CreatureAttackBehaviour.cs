using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureAttackBehaviour : Behaviour
    {
        private IAttackStrategy attackStrategy;

        public CreatureAttackBehaviour(IAttackStrategy attackStrategy)
        {
            this.attackStrategy = attackStrategy;
        }

        private Guid globalTick;

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            DateTime lastCreatureAttack = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (DateTime.UtcNow > lastCreatureAttack)
                {
                    if (attackStrategy.CanAttack(creature, null) )
                    {
                        lastCreatureAttack = DateTime.UtcNow.Add(attackStrategy.Cooldown);

                        return attackStrategy.Attack(creature, null);
                    }

                    lastCreatureAttack = DateTime.UtcNow.AddMilliseconds(500);
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}