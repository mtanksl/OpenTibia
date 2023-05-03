using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components.Conversations
{
    public abstract class TopicCondition
    {
        public virtual AggregateTopicCondition And(TopicCondition topicCondition)
        {
            return new AggregateTopicCondition(this, topicCondition);
        }

        public abstract bool Handle(Conversation conversation, Npc npc, Player player, string message);
    }
}