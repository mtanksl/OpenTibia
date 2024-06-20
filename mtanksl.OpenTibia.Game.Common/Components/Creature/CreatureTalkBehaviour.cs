using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
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

        private DateTime nextTalk = DateTime.MinValue;

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[creature.Id % 10], (context, e) =>
            {
                if (DateTime.UtcNow >= nextTalk)
                {
                    nextTalk = DateTime.UtcNow.Add(TimeSpan.FromSeconds(30) );

                    return Context.AddCommand(new ShowTextCommand(creature, talkType, Context.Server.Randomization.Take(sentences) ) );
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Creature creature = (Creature)GameObject;

            Context.Server.EventHandlers.Unsubscribe(GlobalTickEventArgs.Instance[creature.Id % 10], globalTick);
        }
    }
}