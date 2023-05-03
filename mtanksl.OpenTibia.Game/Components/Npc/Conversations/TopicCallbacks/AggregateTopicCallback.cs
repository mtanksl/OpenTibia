using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Components.Conversations
{
    public class AggregateTopicCallback : TopicCallback
    {
        private List<TopicCallback> topicCallbacks = new List<TopicCallback>();

        public AggregateTopicCallback(params TopicCallback[] topicCallbacks)
        {
            this.topicCallbacks.AddRange(topicCallbacks);
        }

        public void Add(TopicCallback topicCallback)
        {
            topicCallbacks.Add(topicCallback);
        }

        public override AggregateTopicCallback And(TopicCallback topicCallback)
        {
            Add(topicCallback);

            return this;
        }

        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            foreach (var topicCondition in topicCallbacks)
            {
               await topicCondition.Handle(conversation, npc, player);
            }
        }
    }
}