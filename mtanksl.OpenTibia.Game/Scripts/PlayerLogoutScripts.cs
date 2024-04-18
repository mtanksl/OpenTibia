using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLogoutScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new PlayerLogoutScriptingHandler() );

            Context.Server.EventHandlers.Subscribe(new PlayerLogoutVipHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}