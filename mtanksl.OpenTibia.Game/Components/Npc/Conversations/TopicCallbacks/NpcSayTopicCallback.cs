using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Text;

namespace OpenTibia.Game.Components.Conversations
{
    public class NpcSay : TopicCallback
    {
        private string answer;

        public NpcSay(string answer)
        {
            this.answer = answer;
        }

        public override Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            if (answer.Contains("@") )
            {
                StringBuilder builder = new StringBuilder(answer);

                foreach (var item in conversation.Data)
                {
                    builder.Replace("@" + item.Key, item.Value.ToString() );
                }

                return Context.Current.AddCommand(new NpcSayCommand(npc, builder.ToString() ) );
            }

            return Context.Current.AddCommand(new NpcSayCommand(npc, answer) );
        }
    }
}