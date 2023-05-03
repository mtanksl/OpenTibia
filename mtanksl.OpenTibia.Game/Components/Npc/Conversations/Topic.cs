using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components.Conversations
{
    public class Topic
    {
        public Topic(TopicCondition condition, TopicCallback callback)
        {
            Condition = condition;

            Callback = callback;
        }

        public TopicCondition Condition { get; set; }

        public TopicCallback Callback { get; set; }

        public Topic AddCondition(Func<Conversation, Npc, Player, string, bool> handle)
        {
            return AddCondition(new InlineTopicCondition(handle) );
        }

        public Topic AddCondition(TopicCondition condition)
        {
            Condition = Condition.And(condition);

            return this;
        }

        public Topic AddCallback(Func<Conversation, Npc, Player, Promise> handle)
        {
            return AddCallback(new InlineTopicCallback(handle) );
        }

        public Topic AddCallback(TopicCallback callback)
        {
            Callback = Callback.And(callback);

            return this;
        }
    }
}