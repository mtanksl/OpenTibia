using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerRotateItemScripts : Script
    {
        public override void Start(Server server)
        {
            server.CommandHandlers.Add(new RotateItemWalkToSourceHandler() );

            server.CommandHandlers.Add(new RotateItemTransformHandler() );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}