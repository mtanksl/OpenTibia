using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class TalkBehaviour : PeriodicBehaviour
    {
        private string[] sentences;

        public TalkBehaviour(string[] sentences)
        {
            this.sentences = sentences;
        }

        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "Talk_Behaviour_" + creature.Id;
        }

        private bool running = false;

        public override async Promise Update()
        {
            if (!running)
            {
                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (creature.Tile.Position.CanHearSay(observer.Tile.Position) )
                    {
                        running = true;

                        await Context.AddCommand(new ShowTextCommand(creature, TalkType.MonsterSay, Context.Server.Randomization.Take(sentences) ) );
                        
                        await Promise.Delay(key, 30000);

                        running = false;

                        await Update();

                        break;
                    }
                }
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}