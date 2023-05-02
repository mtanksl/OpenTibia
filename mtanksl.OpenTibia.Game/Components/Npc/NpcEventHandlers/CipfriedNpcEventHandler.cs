using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class CipfriedNpcEventHandler : NpcEventHandler
    {
        public override Promise OnGreet(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Hello, " + player.Name + "! Feel free to ask me for help.") );
        }

        public override Promise OnGreetBusy(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Please wait, " + player.Name + ". I already talk to someone!") );
        }

        public override Promise OnSay(Npc npc, Player player, string message)
        {
            if (message == "name")
            {
                return Context.Current.AddCommand(new NpcSayCommand(npc, "My name is Cipfried.") );
            }
            else if (message == "job")
            {
                return Context.Current.AddCommand(new NpcSayCommand(npc, "I am just a humble monk. Ask me if you need help or healing.") );
            }
            else if (message == "monk")
            {
                return Context.Current.AddCommand(new NpcSayCommand(npc, "I sacrifice my life to serve the good gods of Tibia.") );
            }
            else if (message == "tibia")
            {
                return Context.Current.AddCommand(new NpcSayCommand(npc, "That's where we are. The world of Tibia.") );
            }

            return Promise.Completed;
        }

        public override Promise OnFarewell(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Farewell, " + player.Name + "!") );
        }

        public override Promise OnDisappear(Npc npc)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Well, bye then.") );
        }
    }
}