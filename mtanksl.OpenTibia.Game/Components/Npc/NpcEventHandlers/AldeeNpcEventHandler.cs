using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class AldeeNpcEventHandler : NpcEventHandler
    {
        public override Promise OnGreet(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Hello, hello, " + player.Name + "! Please come in, look, and buy!") );
        }

        public override Promise OnGreetBusy(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "I'll be with you in a moment, " + player.Name + ".") );
        }

        public override Promise OnSay(Npc npc, Player player, string message)
        {
            if (message == "name")
            {
                return Context.Current.AddCommand(new NpcSayCommand(npc, "My name is Al Dee, but you can call me Al. Do you want to buy something?" ) );
            }
            else if (message == "job")
            {
                return Context.Current.AddCommand(new NpcSayCommand(npc, "I am a merchant. What can I do for you?" ) );
            }

            return Promise.Completed;
        }

        public override Promise OnFarewell(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Bye, bye." ) );
        }

        public override Promise OnDisappear(Npc npc)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Bye, bye.") );
        }
    }
}