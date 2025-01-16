using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerAdvanceLevelScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<PlayerAdvanceLevelEventArgs>(new PlayerAdvanceLevelScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}