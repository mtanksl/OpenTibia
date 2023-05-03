using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components.Conversations
{
    public abstract class TopicCallback
    {
        public abstract Promise Handle(Conversation conversation, Npc npc, Player player);

        public AggregateTopicCallback And(TopicCallback topicCallback)
        {
            if (topicCallback is AggregateTopicCallback aggregate)
            {
                aggregate.Add(topicCallback);

                return aggregate;
            }

            return new AggregateTopicCallback(this, topicCallback);
        }
    }
}