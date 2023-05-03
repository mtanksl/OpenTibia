using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components.Conversations;

namespace OpenTibia.Game.Components
{
    public class CipfriedNpcEventHandler : NpcEventHandler
    {
        private Conversation conversation = new Conversation();

        public CipfriedNpcEventHandler()
        {
            conversation.AddTopic()
                .AddCondition(new MessageMatch("name") )
                .AddCallback(new NpcSay("My name is Cipfried.") );
         
            conversation.AddTopic()
                .AddCondition(new MessageMatch("job") )
                .AddCallback(new NpcSay("I am just a humble monk. Ask me if you need help or healing.") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("monk") )
                .AddCallback(new NpcSay("I sacrifice my life to serve the good gods of Tibia.") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("tibia") )
                .AddCallback(new NpcSay("That's where we are. The world of Tibia.") );
        }

        public override Promise OnGreet(Npc npc, Player player)
        {
            conversation.Data.Clear();

            return Context.Current.AddCommand(new NpcSayCommand(npc, "Hello, " + player.Name + "! Feel free to ask me for help.") );
        }

        public override Promise OnGreetBusy(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Please wait, " + player.Name + ". I already talk to someone!") );
        }

        public override Promise OnSay(Npc npc, Player player, string message)
        {
            return conversation.Handle(npc, player, message);
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