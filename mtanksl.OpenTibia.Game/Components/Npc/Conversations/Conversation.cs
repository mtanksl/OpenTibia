using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Components.Conversations
{
    public class Conversation
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public Dictionary<string, object> Data
        {
            get 
            {
                return data; 
            }
        }
        
        private List<Topic> topics = new List<Topic>();

        public Topic AddTopic()
        {
            return AddTopic(new Topic(new AggregateTopicCondition(), new AggregateTopicCallback() ) );
        }

        public Topic AddTopic(TopicCondition condition, TopicCallback callback)
        {
            return AddTopic(new Topic(condition, callback) );
        }

        public Topic AddTopic(Topic topic)
        {
            topics.Add(topic);

            return topic;
        }

        public Promise Handle(Npc npc, Player player, string message)
        {
            foreach (var topic in topics)
            {
                if (topic.Condition.Handle(this, npc, player, message) )
                {
                    return topic.Callback.Handle(this, npc, player);
                }
            }

            return Promise.Completed;
        }
    }
}