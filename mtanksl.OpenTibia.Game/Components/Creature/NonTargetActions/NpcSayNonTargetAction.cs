using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcSayNonTargetAction : NonTargetAction
    {
        private string[] sentences;

        private TimeSpan cooldown;

        public NpcSayNonTargetAction(string[] sentences, TimeSpan cooldown)
        {
            this.sentences = sentences;

            this.cooldown = cooldown;
        }

        private DateTime talkCooldown;

        public override Promise Update(Creature creature)
        {
            if (DateTime.UtcNow > talkCooldown)
            {
                talkCooldown = DateTime.UtcNow.Add(cooldown);

                return Context.Current.AddCommand(new NpcSayCommand( (Npc)creature, Context.Current.Server.Randomization.Take(sentences) ) );
            }

            return Promise.Completed;
        }
    }
}