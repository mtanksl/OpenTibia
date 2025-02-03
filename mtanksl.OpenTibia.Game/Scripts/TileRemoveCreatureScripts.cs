using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class TileRemoveCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<TileRemoveCreatureCommand>(new TileRemovingCreatureScriptingHandler() );


            Context.Server.EventHandlers.Subscribe<TileRemoveCreatureEventArgs>(new TileRemoveCreatureScriptingHandler() );

            Context.Server.EventHandlers.Subscribe<TileRemoveCreatureEventArgs>(new TileDepressHandler() );

            Context.Server.EventHandlers.Subscribe<TileRemoveCreatureEventArgs>(new CloseDoorAutomaticallyHandler() );

            Context.Server.EventHandlers.Subscribe<TileRemoveCreatureEventArgs>(new SwimLeaveHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}