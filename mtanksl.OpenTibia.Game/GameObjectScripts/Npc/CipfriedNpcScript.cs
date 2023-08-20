using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class CipfriedNpcScript : NpcScript
    {
        public override string Key
        {
            get
            {
                return "Cipfried";
            }
        }

        public override void Start(Npc npc)
        {
            base.Start(npc);

            var builder = new ConversationStrategyBuilder()
                .WithGreeting("Hello, {player.Name}! Feel free to ask me for help.")
                .WithBusy("Please wait, {player.Name}. I already talk to someone!")
                .WithSay("name", "My name is Cipfried.")
                .WithSay("job", "I am just a humble monk. Ask me if you need help or healing.")
                .WithSay("monk", "I sacrifice my life to serve the good gods of Tibia.")
                .WithSay("tibia", "That's where we are. The world of Tibia.")
                .WithFarewell("Farewell, {player.Name}!")
                .WithDismiss("Well, bye then.");

            Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(builder.Build(), new RandomWalkStrategy() ) );
        }

        public override void Stop(Npc npc)
        {
            base.Stop(npc);


        }
    }
}