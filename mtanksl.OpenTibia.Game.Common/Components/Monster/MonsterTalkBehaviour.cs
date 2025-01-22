using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class MonsterTalkBehaviour : Behaviour
    {
        private VoiceCollection voices;

        public MonsterTalkBehaviour(VoiceCollection voices)
        {
            this.voices = voices;
        }

        private Guid globalTick;

        public override void Start()
        {
            Monster monster = (Monster)GameObject;

            int ticks = voices.Interval;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(monster.Id), async (context, e) =>
            {
                ticks -= e.Ticks;

                while (ticks <= 0)
                {
                    ticks += voices.Interval;

                    if (Context.Server.Randomization.HasProbability(voices.Chance / 100.0) )
                    {
                        VoiceItem voiceItem = Context.Server.Randomization.Take(voices.Items);

                        if (voiceItem.Yell)
                        {
                            await Context.AddCommand(new MonsterYellCommand(monster, voiceItem.Sentence) );
                        }
                        else
                        {
                            await Context.AddCommand(new MonsterSayCommand(monster, voiceItem.Sentence) );
                        }
                    }
                }
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}