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

        private Monster monster;

        private Guid creatureAppearEventArgs;

        private Guid creatureDisappearEventArgs;

        private int near;

        public override void Start()
        {
            monster = (Monster)GameObject;

            creatureAppearEventArgs = Context.Server.GameObjectEventHandlers.Subscribe<CreatureAppearEventArgs>(monster, (context, e) => 
            {
                if (e.Creature is Player player)
                {
                    if (near == 0)
                    {
                        ticks = voices.Interval;

                        globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(monster.Id), OnThink);
                    }

                    near++;
                }

                return Promise.Completed; 
            } );

            creatureDisappearEventArgs = Context.Server.GameObjectEventHandlers.Subscribe<CreatureDisappearEventArgs>(monster, (context, e) =>
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

        private int ticks;

        private Guid globalTick;

        private async Promise OnThink(Context context, GlobalTickEventArgs e)
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