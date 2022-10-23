using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;

namespace OpenTibia.Game.Components
{
    public class AttackBehaviour : PeriodicBehaviour
    {
        private IAttackStrategy attackStrategy;

        public AttackBehaviour(IAttackStrategy attackStrategy)
        {
            this.attackStrategy = attackStrategy;
        }

        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "Attack_Behaviour_" + creature.Id;
        }

        private bool running = false;

        public override void Update(Context context)
        {
            if (running)
            {
                return;
            }

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.CanHearSay(observer.Tile.Position) )
                {
                    var command = attackStrategy.GetNext(context, creature, observer);

                    if (command != null)
                    {
                        running = true;

                        context.AddCommand(command).Then(ctx =>
                        {
                            return Promise.Delay(ctx.Server, key, attackStrategy.CooldownInMilliseconds);

                        } ).Then(ctx =>
                        {
                            running = false;

                            Update(ctx);
                        } );
                    }

                    break;
                }
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}