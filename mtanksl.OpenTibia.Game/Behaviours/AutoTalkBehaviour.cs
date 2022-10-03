using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;

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

        private bool running = false;

        public override void Update(Context context)
        {
            if (running)
            {
                return;
            }

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.IsInBattleRange(observer.Tile.Position) )
                {
                    running = true;

                    context.AddCommand(new ShowTextCommand(creature, TalkType.MonsterSay, sentences.Random() ) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new DelayCommand(Constants.CreatureTalkSchedulerEvent(creature), Constants.CreatureTalkSchedulerEventInterval) );

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
            server.CancelQueueForExecution(Constants.CreatureTalkSchedulerEvent(creature) );
        }
    }
}