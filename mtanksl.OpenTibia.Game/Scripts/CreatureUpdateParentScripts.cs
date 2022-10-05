using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureUpdateParentScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new MagicForcefieldHandler() );

            server.CommandHandlers.Add(new HoleHandler() );

            server.CommandHandlers.Add(new StairsHandler() );

            server.CommandHandlers.Add(new TileDepressHandler() );

            server.CommandHandlers.Add(new TilePressHandler() );

            server.CommandHandlers.Add(new SnowPressHandler() );

            server.CommandHandlers.Add(new MoveAwayFromContainerCloseHandler() );        
        }

        public void Stop(Server server)
        {
            
        }
    }
}