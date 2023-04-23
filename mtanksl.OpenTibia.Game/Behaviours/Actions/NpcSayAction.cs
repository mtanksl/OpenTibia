using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcSayAction : BehaviourAction
    {
        private string[] sentences;

        public NpcSayAction(string[] sentences)
        {
            this.sentences = sentences;
        }

        private DateTime talkCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > talkCooldown)
            {
                talkCooldown = DateTime.UtcNow.AddSeconds(30);

                return Context.Current.AddCommand(new NpcSayCommand( (Npc)attacker, Context.Current.Server.Randomization.Take(sentences) ) );
            }

            return Promise.Completed;
        }
    }
}