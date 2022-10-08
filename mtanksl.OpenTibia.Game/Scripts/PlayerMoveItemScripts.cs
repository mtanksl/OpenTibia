using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerMoveItemScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MoveItemWalkToSourceHandler() );

            server.CommandHandlers.Add(new ThrowAwayContainerCloseHandler() );        

            server.CommandHandlers.Add(new DustbinHandler() );

            server.CommandHandlers.Add(new ShallowWaterHandler() );

            server.CommandHandlers.Add(new SwampHandler() );

            server.CommandHandlers.Add(new LavaHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}