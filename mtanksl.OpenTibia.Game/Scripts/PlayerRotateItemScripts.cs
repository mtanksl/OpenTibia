using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerRotateItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new RotateItemWalkToSourceHandler() );

            Context.Server.CommandHandlers.Add(new RotateItemTransformHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}