using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class CreatureDeathScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<CreatureDeathEventArgs>(new CreatureDeathScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}