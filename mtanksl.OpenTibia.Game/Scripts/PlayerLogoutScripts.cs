using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLogoutScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new PlayerLogoutScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}