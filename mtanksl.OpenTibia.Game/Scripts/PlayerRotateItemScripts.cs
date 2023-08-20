using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerRotateItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new RotateItemWalkToSourceHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new RotateItemTransformHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}