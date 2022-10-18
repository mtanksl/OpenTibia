using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;

namespace OpenTibia.Game.Components
{
    public class RandomTalkBehaviour : PeriodicBehaviour
    {
        private string[] sentences;

        public RandomTalkBehaviour(string[] sentences)
        {
            this.sentences = sentences;
        }

        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "Random_Talk_Behaviour_" + creature.Id;
        }

        private bool running = false;

        public override void Update(Context context)
        {
            if (running)
            {
                return;
            }

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.CanHearSay(observer.Tile.Position) )
                {
                    running = true;

                    context.AddCommand(new ShowTextCommand(creature, TalkType.MonsterSay, sentences.Random() ) ).Then(ctx =>
                    {
                        return Promise.Delay(ctx.Server, key, 30000);

                    } ).Then(ctx =>
                    {
                        running = false;

                        Update(ctx);
                    } );

                    break;
                }
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}