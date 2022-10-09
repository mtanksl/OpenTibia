using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class TileAddCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.EventHandlers.Subscribe(new TilePressHandler() );

            server.EventHandlers.Subscribe(new SnowPressHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}