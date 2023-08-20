using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureTalkBehaviour : Behaviour
    {
        private TalkType talkType;

        private string[] sentences;

        public CreatureTalkBehaviour(TalkType talkType, string[] sentences)
        {
            this.talkType = talkType;

            this.sentences = sentences;
        }

        private Guid globalTick;

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            DateTime lastTalk = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (DateTime.UtcNow > lastTalk)
                {
                    lastTalk = DateTime.UtcNow.Add(TimeSpan.FromSeconds(30) );

                    return Context.AddCommand(new ShowTextCommand(creature, talkType, Context.Server.Randomization.Take(sentences) ) );
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