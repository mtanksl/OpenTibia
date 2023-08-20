using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class MonsterThinkBehaviour : Behaviour
    {
        private Guid globalTick;

        public override void Start()
        {
            Monster monster = (Monster)GameObject;

            Player target = null;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (target == null || target.Tile == null || target.IsDestroyed || !monster.Tile.Position.CanSee(target.Tile.Position) )
                {
                    Player[] players = Context.Server.Map.GetObservers(monster.Tile.Position).OfType<Player>().Where(p => p.Vocation != Vocation.Gamemaster && monster.Tile.Position.CanHearSay(p.Tile.Position) ).ToArray();

                    if (players.Length > 0)
                    {
                        target = Context.Server.Randomization.Take(players);
                    }
                    else
                    {
                        target = null;
                    }
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