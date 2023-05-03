using OpenTibia.Common.Objects;
using System;
using System.Text.RegularExpressions;

namespace OpenTibia.Game.Components.Conversations
{
    public class MessageMatchRegex<T> : TopicCondition
    {
        private string question;

        public MessageMatchRegex(string question)
        {
            this.question = question;
        }

        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            var match = Regex.Match(message, question);

            if (match.Success)
            {
                conversation.Data["Group"] = Convert.ChangeType(match.Groups[1].Value, typeof(T) );

                return true;
            }

            return false;
        }
    }
}