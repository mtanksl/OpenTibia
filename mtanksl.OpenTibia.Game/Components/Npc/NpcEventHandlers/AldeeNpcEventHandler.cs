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
                .AddCondition(new MessageMatch("wares") )
                .AddCallback(new NpcSay("I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("offer") )
                .AddCallback(new NpcSay("I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?") );

            foreach (var item in new (string Question, string Name, int OpenTibiaId, int Price)[] 
            {
                ("sell axe", "an axe", 2386, 7),
                ("sell sword", "a sword", 2376, 25),
                ("sell mace", "a mace", 2398, 30)
            } )
            {
                conversation.AddTopic()

                    .AddCondition(new MessageMatch(item.Question) )

                    .AddCallback(new ParametersUpdate(new { Type = item.OpenTibiaId, Amount = 1, Price = item.Price, Topic = 2 } ) )

                    .AddCallback(new NpcSay("Do you want to sell " + item.Name + " for @Price gold?") );

            }

            foreach (var item in new (string Question, string Name, int OpenTibiaId, int Price)[] 
            {
                ("sell (\\d+) axe", "axes", 2386, 7),
                ("sell (\\d+) sword", "swords", 2376, 25),
                ("sell (\\d+) mace", "maces", 2398, 30)
            } )
            {
                conversation.AddTopic()

                    .AddCondition(new MessageMatchRegex<int>(item.Question) )

                    .AddCondition(new GroupMatchRegex<int>(match => match >= 1 && match <= 100) )

                    .AddCallback(new ParametersUpdateRegex<int>(match => new { Type = item.OpenTibiaId, Amount = match, Price = item.Price * match, Topic = 2 } ) )

                    .AddCallback(new NpcSay("Do you want to sell @Amount " + item.Name + " for @Price gold?") );
            }

            foreach (var item in new (string Question, string Name, int OpenTibiaId, int Price)[] 
            {
                ("axe", "an axe", 2386, 20)
            } )
            {
                conversation.AddTopic()

                    .AddCondition(new MessageMatch(item.Question) )

                    .AddCallback(new ParametersUpdate(new { Type = item.OpenTibiaId, Amount = 1, Price = item.Price, Topic = 1 } ) )

                    .AddCallback(new NpcSay("Do you want to buy " + item.Name + " for @Price gold?") );

            }

            foreach (var item in new (string Question, string Name, int OpenTibiaId, int Price)[] 
            {
                ("(\\d+) axe", "axes", 2386, 20)
            } )
            {
                conversation.AddTopic()

                    .AddCondition(new MessageMatchRegex<int>(item.Question) )

                    .AddCondition(new GroupMatchRegex<int>(match => match >= 1 && match <= 100) )

                    .AddCallback(new ParametersUpdateRegex<int>(match => new { Type = item.OpenTibiaId, Amount = match, Price = item.Price * match, Topic = 1 } ) )

                    .AddCallback(new NpcSay("Do you want to buy @Amount " + item.Name + " for @Price gold?") );
            }
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