using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLoginScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new PlayerLoginScriptingHandler() );

            Context.Server.EventHandlers.Subscribe(new PlayerLoginVipHandler() );

            Context.Server.EventHandlers.Subscribe(new WelcomeHandler() );

            Context.Server.EventHandlers.Subscribe(new AccountManagerLoginHandler() );

            Context.Server.EventHandlers.Subscribe(new RecordHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}