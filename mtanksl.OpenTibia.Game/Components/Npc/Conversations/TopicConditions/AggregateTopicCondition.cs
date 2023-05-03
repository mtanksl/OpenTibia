using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Components.Conversations
{
    public class AggregateTopicCondition : TopicCondition
    {
        private List<TopicCondition> topicConditions = new List<TopicCondition>();

        public AggregateTopicCondition(params TopicCondition[] topicConditions)
        {
            this.topicConditions.AddRange(topicConditions);
        }

        public void Add(TopicCondition topicCondition)
        {
            topicConditions.Add(topicCondition);
        }

        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            foreach (var topicCondition in topicConditions)
            {
                if ( !topicCondition.Handle(conversation, npc, player, message) )
                {
                    return false;
                }
            }

            return true;
        }
    }
}