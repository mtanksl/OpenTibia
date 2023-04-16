using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class CreatureTalkThinkBehaviour : ThinkBehaviour
    {
        private string[] sentences;

        public CreatureTalkThinkBehaviour(string[] sentences)
        {
            this.sentences = sentences;
        }

        private Creature creature;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;
        }

        private DateTime talkCooldown;

        public override Promise Update()
        {
            if (DateTime.UtcNow > talkCooldown)
            {
                var target = Context.Server.GameObjects.GetPlayers()
                    .Where(p => creature.Tile.Position.CanHearSay(p.Tile.Position) )
                    .FirstOrDefault();

                if (target == null)
                {
                    talkCooldown = DateTime.UtcNow.AddSeconds(30);

                    return Context.AddCommand(new ShowTextCommand(creature, TalkType.MonsterSay, Context.Server.Randomization.Take(sentences) ) );
                }
                else
                {
                    talkCooldown = DateTime.UtcNow.AddSeconds(30);
                }
            }

            return Promise.Completed;
        }

        public override void Stop(Server server)
        {

        }
    }
}