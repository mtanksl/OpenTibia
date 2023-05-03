using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components.Conversations
{
    public abstract class TopicCondition
    {
        public abstract bool Handle(Conversation conversation, Npc npc, Player player, string message);

        public AggregateTopicCondition And(TopicCondition topicCondition)
        {
            if (topicCondition is AggregateTopicCondition aggregate)
            {
                aggregate.Add(topicCondition);

                return aggregate;
            }

            return new AggregateTopicCondition(this, topicCondition);
        }
    }
}