using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcSayAction : BehaviourAction
    {
        private string[] sentences;

        private TimeSpan cooldown;

        public NpcSayAction(string[] sentences, TimeSpan cooldown)
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

                return Context.Current.AddCommand(new NpcSayCommand( (Npc)attacker, Context.Current.Server.Randomization.Take(sentences) ) );
            }

            return Promise.Completed;
        }
    }
}