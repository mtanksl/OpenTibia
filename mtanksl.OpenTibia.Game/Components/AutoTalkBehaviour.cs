using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System;

namespace OpenTibia.Game.Components
{
    public class AutoTalkBehaviour : TimeBehaviour
    {
        private string[] sentences;

        public AutoTalkBehaviour(string[] sentences)
        {
            this.sentences = sentences;
        }

        private Creature creature;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;
        }

        public override void Stop(Server server)
        {
            creature = null;
        }

        private DateTime next;

        public override void Update(Context context)
        {
            if (next < DateTime.Now)
            {
                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (creature.Tile.Position.IsInBattleRange(observer.Tile.Position) )
                    {
                        context.AddCommand(new TextCommand(creature, TalkType.MonsterSay, sentences.Random() ) );

                        break;
                    }
                }

                next = DateTime.Now.AddSeconds(10);
            }
        }        
    }
}