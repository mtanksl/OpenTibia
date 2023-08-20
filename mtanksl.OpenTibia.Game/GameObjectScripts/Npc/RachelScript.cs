using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class RachelScript : NpcScript
    {
        public override string Key
        {
            get
            {
                return "Rachel";
            }
        }

        public override void Start(Npc npc)
        {
            base.Start(npc);

            var builder = new ConversationStrategyBuilder()
                .WithGreeting("Welcome {player.Name}! Whats your need?")
                .WithBusy("Wait, {player.Name}! One after the other.")
                .WithSay("name", "I am the illusterous Rachel, of course.")
                .WithSay("job", "I am the head alchemist of Carlin. I keep the secret recipies of our ancestors. Besides, I am selling mana and life fluids, spellbooks, wands, rods and runes.")
                .WithSay("rune", "I sell blank runes and spell runes.")
                .WithSay("runes", "I sell blank runes and spell runes.")
                .WithFarewell("Good bye,{player.Name}.")
                .WithDismiss("These impatient young brats!");

            Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(builder.Build(), new RandomWalkStrategy(2) ) );
        }

        public override void Stop(Npc npc)
        {
            base.Stop(npc);


        }
    }
}