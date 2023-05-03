using OpenTibia.Common.Objects;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.Components.Conversations
{
    public class InlineTopicCondition : TopicCondition
    { 
        private Func<Conversation, Npc, Player, string, bool> handle;

        public InlineTopicCondition(Func<Conversation, Npc, Player, string, bool> handle)
        {
            this.handle = handle;
        }

        [DebuggerStepThrough]
        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            return handle(conversation, npc, player, message);
        }       
    }
}