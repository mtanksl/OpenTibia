using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLoginScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new PlayerLoginScriptingHandler() );

            Context.Server.EventHandlers.Subscribe(new WelcomeHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}