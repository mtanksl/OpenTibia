using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class TileRemoveCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new TileRemoveCreatureScriptingHandler() );

            Context.Server.EventHandlers.Subscribe(new TileDepressHandler() );

            Context.Server.EventHandlers.Subscribe(new CloseDoorAutomaticallyHandler() );

            Context.Server.EventHandlers.Subscribe(new SwimLeaveHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}