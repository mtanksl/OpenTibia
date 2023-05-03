using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components.Conversations;

namespace OpenTibia.Game.Components
{
    public class RachelNpcEventHandler : NpcEventHandler
    {
        private Conversation conversation = new Conversation();

        public RachelNpcEventHandler()
        {
            // Topic 1

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 1) )
                .AddCondition(new MessageMatch("yes") )
                .AddCondition(new PlayerHasMoney() )
                .AddCallback(new PlayerRemoveMoney() )
                .AddCallback(new PlayerAddItem() )
                .AddCallback(new NpcSay("Here you are.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 1) )
                .AddCondition(new MessageMatch("yes") )
                .AddCallback(new NpcSay("Sorry, you don't have enough gold.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            conversation.AddTopic()
                .AddCondition(new ParametersMatch("Topic", 1) )
                .AddCallback(new NpcSay("As you wish.") )
                .AddCallback(new ParametersUpdate(new { Topic = 0 } ) );

            //

            conversation.AddTopic()
                .AddCondition(new MessageMatch("name") )
                .AddCallback(new NpcSay("I am the illusterous Rachel, of course.") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("job") )
                .AddCallback(new NpcSay("I am the head alchemist of Carlin. I keep the secret recipies of our ancestors. Besides, I am selling mana and life fluids, spellbooks, wands, rods and runes.") );

            conversation.AddTopic()
                .AddCondition(new MessageMatch("rune") )
                .AddCallback(new NpcSay("I sell blank runes and spell runes.") );

            foreach (var item in new (string Question, string Name, int OpenTibiaId, int Count, int Price)[] 
            {
                ("light magic missile rune", "a light magic missile rune", 2311, 5, 40)
            } )
            {
                conversation.AddTopic()

                    .AddCondition(new MessageMatch(item.Question) )

                    .AddCallback(new ParametersUpdate(new { Type = item.OpenTibiaId, Data = item.Count, Amount = 1, Price = item.Price, Topic = 1 } ) )

                    .AddCallback(new NpcSay("Do you want to buy " + item.Name + " for @Price gold?") );

            }

            foreach (var item in new (string Question, string Name, int OpenTibiaId, int Count, int Price)[] 
            {
                ("(\\d+) light magic missile rune", "light magic missile runes", 2311, 5, 40)
            } )
            {
                conversation.AddTopic()

                    .AddCondition(new MessageMatchRegex<int>(item.Question) )

                    .AddCondition(new GroupMatchRegex<int>(match => match >= 1 && match <= 100) )

                    .AddCallback(new ParametersUpdateRegex<int>(match => new { Type = item.OpenTibiaId, Data = item.Count, Amount = match, Price = item.Price * match, Topic = 1 } ) )

                    .AddCallback(new NpcSay("Do you want to buy @Amount " + item.Name + " for @Price gold?") );
            }
        }

        public override Promise OnGreet(Npc npc, Player player)
        {
            conversation.Data.Clear();

            return Context.Current.AddCommand(new NpcSayCommand(npc, "Welcome " + player.Name + "! Whats your need?") );
        }

        public override Promise OnGreetBusy(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Wait, " + player.Name + "! One after the other.") );
        }

        public override Promise OnSay(Npc npc, Player player, string message)
        {
            return conversation.Handle(npc, player, message);
        }

        public override Promise OnFarewell(Npc npc, Player player)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "Good bye, " + player.Name) );
        }

        public override Promise OnDisappear(Npc npc)
        {
            return Context.Current.AddCommand(new NpcSayCommand(npc, "These impatient young brats!") );
        }
    }
}