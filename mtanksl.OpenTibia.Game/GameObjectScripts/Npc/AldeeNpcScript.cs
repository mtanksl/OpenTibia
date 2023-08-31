using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class AldeeNpcScript : NpcScript
    {
        public override string Key
        {
            get
            {
                return "Aldee";
            }
        }

        public override void Start(Npc npc)
        {
            base.Start(npc);

            /*
            var builder = new ConversationStrategyBuilder()
                .WithGreeting("Hello, hello, {player.Name}! Please come in, look, and buy!")
                .WithBusy("I'll be with you in a moment, {player.Name}.")
                .WithSay("name", "My name is Al Dee, but you can call me Al. Do you want to buy something?")
                .WithSay("job", "I am a merchant. What can I do for you?")
                .WithSay("wares", "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?")
                .WithSay("offer", "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?")
                .WithFarewell("Bye, bye.")
                .WithDismiss("Bye, bye.");

            Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(builder.Build(), new RandomWalkStrategy(2) ) );
            */

            Context.Server.GameObjectComponents.AddComponent(npc, new NpcScriptingBehaviour("aldee.lua", new RandomWalkStrategy(2) ) );
        }

        public override void Stop(Npc npc)
        {
            base.Stop(npc);


        }
    }
}