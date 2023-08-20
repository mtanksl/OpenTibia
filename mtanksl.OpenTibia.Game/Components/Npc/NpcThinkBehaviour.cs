using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class NpcThinkBehaviour : Behaviour
    {
        private IConversationStrategy conversationStrategy;

        private IWalkStrategy walkStrategy;

        public NpcThinkBehaviour(IConversationStrategy conversationStrategy, IWalkStrategy walkStrategy)
        {
            this.conversationStrategy = conversationStrategy;

            this.walkStrategy = walkStrategy;
        }

        private Guid playerSay;

        private Guid globalTick;

        public override void Start()
        {
            Npc npc = (Npc)GameObject;

            DateTime lastSentence = DateTime.MinValue;

            QueueHashSet<Player> targets = new QueueHashSet<Player>();

            playerSay = Context.Server.EventHandlers.Subscribe<PlayerSayEventArgs>( (context, e) =>
            {
                //TODO

                return Promise.Completed;
            } );

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                //TODO

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<PlayerSayEventArgs>(playerSay);

            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}