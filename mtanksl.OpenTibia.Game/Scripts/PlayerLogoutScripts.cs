using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLogoutScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<PlayerLogoutEventArgs>(new PlayerLogoutScriptingHandler() );

            Context.Server.EventHandlers.Subscribe<PlayerLogoutEventArgs>(new PlayerLogoutVipHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}