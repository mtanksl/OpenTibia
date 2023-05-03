using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components.Conversations
{
    public abstract class TopicCallback
    {
        public virtual AggregateTopicCallback And(TopicCallback topicCallback)
        {
            return new AggregateTopicCallback(this, topicCallback);
        }

        public abstract Promise Handle(Conversation conversation, Npc npc, Player player);
    }
}