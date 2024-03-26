using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerRotateItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new RotateItemWalkToSourceHandler() ); //TODO: Re-validate rules for incoming packet
                        
            Context.Server.CommandHandlers.AddCommandHandler(new RotateItemScriptingHandler() );


            Context.Server.CommandHandlers.AddCommandHandler(new RotateItemTransformHandler() );
        }

        public override void Stop()
        {

        }
    }   
}