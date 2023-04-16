using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class CreatureTalkBehaviour : Behaviour
    {
        private string[] sentences;

        public CreatureTalkBehaviour(string[] sentences)
        {
            this.sentences = sentences;
        }

        private Creature creature;

        private Guid token;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalCreatureThinkEventArgs>( (context, e) =>
            {
                return Update();
            } );
        }

        private DateTime talkCooldown;

        private Promise Update()
        {
            if (DateTime.UtcNow > talkCooldown)
            {
                var target = Context.Server.GameObjects.GetPlayers()
                    .Where(p => creature.Tile.Position.CanHearSay(p.Tile.Position) )
                    .FirstOrDefault();

                if (target == null)
                {
                    talkCooldown = DateTime.UtcNow.AddSeconds(30);

                    return Context.AddCommand(new ShowTextCommand(creature, TalkType.MonsterSay, Context.Server.Randomization.Take(sentences) ) );
                }
                else
                {
                    talkCooldown = DateTime.UtcNow.AddSeconds(30);
                }
            }

            return Promise.Completed;
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalCreatureThinkEventArgs>(token);
        }
    }
}