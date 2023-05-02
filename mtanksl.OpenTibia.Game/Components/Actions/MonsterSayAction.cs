using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class MonsterSayAction : BehaviourAction
    {
        private string[] sentences;

        private TimeSpan cooldown;

        public MonsterSayAction(string[] sentences, TimeSpan cooldown)
        {
            this.sentences = sentences;

            this.cooldown = cooldown;
        }

        private DateTime talkCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > talkCooldown)
            {
                talkCooldown = DateTime.UtcNow.Add(cooldown);

                return Context.Current.AddCommand(new ShowTextCommand(attacker, TalkType.MonsterSay, Context.Current.Server.Randomization.Take(sentences) ) );
            }

            return Promise.Completed;
        }
    }
}