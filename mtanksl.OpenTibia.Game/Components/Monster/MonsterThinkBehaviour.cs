using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class MonsterThinkBehaviour : Behaviour
    {
        private TargetAction[] targetActions;

        private NonTargetAction[] nonTargetActions;

        public MonsterThinkBehaviour(TargetAction[] targetActions, NonTargetAction[] nonTargetActions)
        {
            this.targetActions = targetActions;

            this.nonTargetActions = nonTargetActions;
        }

        private Guid globalTick;

        public override void Start(Server server)
        {
            Creature attacker = (Creature)GameObject;

            Player target = null;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>(async (context, e) =>
            {
                if (target == null || target.IsDestroyed || !attacker.Tile.Position.CanHearSay(target.Tile.Position) )
                {
                    Player[] players = server.Map.GetObservers(attacker.Tile.Position).OfType<Player>().Where(p => p.Vocation != Vocation.Gamemaster && attacker.Tile.Position.CanHearSay(p.Tile.Position) ).ToArray();

                    if (players.Length > 0)
                    {
                        target = server.Randomization.Take(players);
                    }
                    else
                    {
                        target = null;
                    }
                }

                if (target != null)
                {
                    foreach (var targetAction in targetActions)
                    {
                        await targetAction.Update(attacker, target);
                    }
                }

                foreach (var nonTargetAction in nonTargetActions)
                {
                    await nonTargetAction.Update(attacker);
                }
            } );
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}