using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureTalkBehaviour : Behaviour
    {
        private string[] sentences;

        public CreatureTalkBehaviour(string[] sentences)
        {
            this.sentences = sentences;
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
                talkCooldown = DateTime.UtcNow.AddSeconds(30);

                return Context.AddCommand(new ShowTextCommand(attacker, TalkType.MonsterSay, Context.Server.Randomization.Take(sentences) ) );
            }

            return Promise.Completed;
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalCreatureThinkEventArgs>(token);
        }
    }
}