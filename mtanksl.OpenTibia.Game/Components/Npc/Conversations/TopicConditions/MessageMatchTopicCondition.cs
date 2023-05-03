using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components.Conversations
{
    public class MessageMatch : TopicCondition
    {
        private string question;

        public MessageMatch(string question)
        {
            this.question = question;
        }

        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            if (message == question)
            {
                return true;
            }

            return false;
        }
    }
}