using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Components.Conversations
{
    public class GroupMatchRegex<T> : TopicCondition
    {
        private Func<T, bool> callback;

        public GroupMatchRegex(Func<T, bool> callback)
        {
            this.callback = callback;
        }

        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            return callback( (T)conversation.Data["Group"] );
        }
    }
}