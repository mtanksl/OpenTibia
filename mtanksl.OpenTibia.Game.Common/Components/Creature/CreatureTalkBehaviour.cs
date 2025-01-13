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

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            int ticks = 30000;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[creature.Id % GlobalTickEventArgs.Instance.Length], (context, e) =>
            {
                ticks -= e.Ticks;

                if (ticks <= 0)
                {
                    ticks += 30000;

                    return Context.AddCommand(new ShowTextCommand(creature, talkType, Context.Server.Randomization.Take(sentences) ) );
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}