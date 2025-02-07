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

        private Npc npc;

        private Guid creatureAppearEventArgs;

        private Guid creatureDisappearEventArgs;

        private int near;

        private int ticks;

        private Guid globalTick;

        public override void Start()
        {
            npc = (Npc)GameObject;

            creatureAppearEventArgs = Context.Server.GameObjectEventHandlers.Subscribe<CreatureAppearEventArgs>(npc, (context, e) => 
            {
                if (e.Creature is Player player)
                {
                    if (near == 0)
                    {
                        ticks = voices.Interval;

                        globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(npc.Id), OnThink);
                    }

                    near++;
                }

                return Promise.Completed; 
            } );

            creatureDisappearEventArgs = Context.Server.GameObjectEventHandlers.Subscribe<CreatureDisappearEventArgs>(npc, (context, e) =>
            {
                if (e.Creature is Player player)
                {
                    near--;

                    if (near == 0)
                    {
                        Context.Server.EventHandlers.Unsubscribe(globalTick);
                    }
                }

                return Promise.Completed;
            } );
        }

        private async Promise OnThink(Context context, GlobalTickEventArgs e)
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
        }

        public override void Stop()
        {
            Context.Server.GameObjectEventHandlers.Unsubscribe(creatureAppearEventArgs);

            Context.Server.GameObjectEventHandlers.Unsubscribe(creatureDisappearEventArgs);

            if (near > 0)
            {
                near = 0;

                Context.Server.EventHandlers.Unsubscribe(globalTick);
            }
        }
    }
}