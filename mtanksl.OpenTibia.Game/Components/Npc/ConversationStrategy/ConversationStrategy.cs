using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class ConversationStrategy : IConversationStrategy
    {
        public string GreetingMessage { get; set; }

        public string BusyMessage { get; set; }

        public Dictionary<string, string> SayMessage { get; set; }

        public string FarewellMessage { get; set; }

        public string DismissMessage { get; set; }

        public Promise Greeting(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, Replace(npc, player, GreetingMessage ?? "Hello {player.Name}.") ) );
        }

        public Promise Busy(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, Replace(npc, player, BusyMessage ?? "I'll talk to you soon {player.Name}.") ) );
        }

        public Promise Say(Npc npc, Player player, string question)
        {
            string answer;

            if ( (SayMessage ?? new Dictionary<string, string>() { { "name", "My name is {npc.Name}." } } ).TryGetValue(question, out answer) )
            {
                return Context.Current.AddCommand(new NpcSayCommand(npc, Replace(npc, player, answer) ) );
            }

            return Promise.Completed;
        }

        public Promise Farewell(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, Replace(npc, player, FarewellMessage ?? "Bye {player.Name}.") ) );
        }

        public Promise Dismiss(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, Replace(npc, player, DismissMessage ?? "Bye.") ) );
        }  

        private string Replace(Npc npc, Player player, string message)
        {
            return message
                .Replace("{npc.Name}", npc.Name)
                .Replace("{player.Name}", player.Name);
        }
    }
}