using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureMoveScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new HoleHandler() );

            server.CommandHandlers.Add(new TileDepressHandler() );

            server.CommandHandlers.Add(new TilePressHandler() );

            server.CommandHandlers.Add(new SnowPressHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}