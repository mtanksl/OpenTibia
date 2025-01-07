using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLoginScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<PlayerLoginEventArgs>(new PlayerLoginScriptingHandler() );

            Context.Server.EventHandlers.Subscribe<PlayerLoginEventArgs>(new PlayerLoginVipHandler() );

            Context.Server.EventHandlers.Subscribe<PlayerLoginEventArgs>(new WelcomeHandler() );

            Context.Server.EventHandlers.Subscribe<PlayerLoginEventArgs>(new AccountManagerLoginHandler() );

            Context.Server.EventHandlers.Subscribe<PlayerLoginEventArgs>(new RecordHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}