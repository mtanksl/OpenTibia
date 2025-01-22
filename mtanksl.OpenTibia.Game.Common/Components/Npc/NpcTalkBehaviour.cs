using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcTalkBehaviour : Behaviour
    {
        private VoiceCollection voices;

        public NpcTalkBehaviour(VoiceCollection voices)
        {
            this.voices = voices;
        }

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            int ticks = voices.Interval;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(npc.Id), async (context, e) =>
            {
                ticks -= e.Ticks;

                while (ticks <= 0)
                {
                    ticks += voices.Interval;

                    if (Context.Server.Randomization.HasProbability(voices.Chance / 100.0) )
                    {
                        VoiceItem voiceItem = Context.Server.Randomization.Take(voices.Items);
                                                                           
                        await Context.AddCommand(new NpcSayCommand(npc, voiceItem.Sentence) );
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