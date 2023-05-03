using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components.Conversations
{
    public class ParametersMatch : TopicCondition
    {
        private string key;

        private object value;

        public ParametersMatch(string key, object value)
        {
            this.key = key;

            this.value = value;
        }

        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            if (conversation.Data.TryGetValue(key, out var temp) && value.Equals(temp) )
            {
                return true;
            }

            return false;
        }
    }
}