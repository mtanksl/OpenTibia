using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.Components.Conversations
{
    public class InlineTopicCallback : TopicCallback
    {
        private Func<Conversation, Npc, Player, Promise> handle;

        public InlineTopicCallback(Func<Conversation, Npc, Player, Promise> handle)
        {
            this.handle = handle;
        }

        [DebuggerStepThrough]
        public override Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            return handle(conversation, npc, player);
        }       
    }
}