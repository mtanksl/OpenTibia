﻿using OpenTibia.Common.Objects;
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

        private int ticks;

        private Guid globalTick;

        public override void Start()
        {
            monster = (Monster)GameObject;

            creatureAppearEventArgs = Context.Server.GameObjectEventHandlers.Subscribe<CreatureAppearEventArgs>(monster, (context, e) => 
            {
                if (e.Creature is Player player)
                {
                    if (near == 0)
                    {
                        StartTalkThread();
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
                        StopTalkThread();
                    }
                }

                return Promise.Completed;
            } );
        }

        private void StartTalkThread()
        {
            ticks = voices.Interval;

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

        private void StopTalkThread()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }

        public override void Stop()
        {
            Context.Server.GameObjectEventHandlers.Unsubscribe(creatureAppearEventArgs);

            Context.Server.GameObjectEventHandlers.Unsubscribe(creatureDisappearEventArgs);

            if (near > 0)
            {
                near = 0;

                StopTalkThread();
            }
        }
    }
}