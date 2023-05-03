using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components.Conversations;

namespace OpenTibia.Game.Components
{
    public class AldeeNpcEventHandler : NpcEventHandler
    {
        private Conversation conversation = new Conversation();

        public AldeeNpcEventHandler()
        {
            // Topic 1

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 1) )
                .AddCondition(new MessageMatch("yes") )
                .AddCondition(new PlayerHasMoney() )
                .AddCallback(new PlayerRemoveMoney() )
                .AddCallback(new PlayerAddItem() )
                .AddCallback(new NpcSay("Thank you. Here it is.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 1) )
                .AddCondition(new MessageMatch("yes") )
                .AddCallback(new NpcSay("Sorry, you do not have enough gold.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 1) )
                .AddCallback(new NpcSay("Maybe you will buy it another time.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            // Topic 2

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 2) )
                .AddCondition(new MessageMatch("yes") )
                .AddCondition(new PlayerHasItem() )
                .AddCallback(new PlayerRemoveItem() )
                .AddCallback(new PlayerAddMoney() )
                .AddCallback(new NpcSay("Ok. Here is your money.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 2) )
                .AddCondition(new MessageMatch("yes") )
                .AddCallback(new NpcSay("Sorry, you do not have so many.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 2) )
                .AddCallback(new NpcSay("Maybe next time.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            //

            conversation.AddTopic()
                .AddCondition(new MessageMatch("name") )
                .AddCallback(new NpcSay("My name is Al Dee, but you can call me Al. Do you want to buy something?") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("job") )
                .AddCallback(new NpcSay("I am a merchant. What can I do for you?") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("sell axe") )
                .AddCallback(new ParametersUpdate(new { Type = 2386, Amount = 1, Price = 7, Topic = 2 } ) )
                .AddCallback(new NpcSay("Do you want to sell an axe for @Price gold?") );

            conversation.AddTopic()
                .AddCondition(new MessageMatchRegex<int>("sell (\\d+) axe") )
                .AddCondition(new GroupMatchRegex<int>(value => value >= 1 && value <= 100) )
                .AddCallback(new ParametersUpdateRegex<int>(value => new { Type = 2386, Amount = value, Price = 7 * value, Topic = 2 } ) )
                .AddCallback(new NpcSay("Do you want to sell @Amount axes for @Price gold?") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("axe") )
                .AddCallback(new ParametersUpdate(new { Type = 2386, Amount = 1, Price = 20, Topic = 1 } ) ) 
                .AddCallback(new NpcSay("Do you want to buy an axe for @Price gold?") );

            conversation.AddTopic()
                .AddCondition(new MessageMatchRegex<int>("(\\d+) axe") )
                .AddCondition(new GroupMatchRegex<int>(value => value >= 1 && value <= 100) )
                .AddCallback(new ParametersUpdateRegex<int>(value => new { Type = 2386, Amount = value, Price = 20 * value, Topic = 1 } ) ) 
                .AddCallback(new NpcSay("Do you want to buy @Amount axes for @Price gold?") );
        }

        public override Promise OnGreet(Npc npc, Player player)
        {
            conversation.Data.Clear();

            return Context.Current.AddCommand(new NpcSayCommand(npc, "Hello, hello, " + player.Name + "! Please come in, look, and buy!") );
        }

        public override Promise OnGreetBusy(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "I'll be with you in a moment, " + player.Name + ".") );
        }

        public override Promise OnSay(Npc npc, Player player, string message)
        {
            return conversation.Handle(npc, player, message);
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