﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class TalkCreatureAction : CreatureAction
    {
        private string[] sentences;

        public TalkCreatureAction(string[] sentences)
        {
            this.sentences = sentences;
        }

        private DateTime talkCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > talkCooldown)
            {
                talkCooldown = DateTime.UtcNow.AddSeconds(30);

                return Context.Current.AddCommand(new ShowTextCommand(attacker, TalkType.MonsterSay, Context.Current.Server.Randomization.Take(sentences) ) );
            }

            return Promise.Completed;
        }
    }
}