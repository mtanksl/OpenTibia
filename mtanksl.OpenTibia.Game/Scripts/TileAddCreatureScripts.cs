using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class TileAddCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.EventHandlers.Subscribe(new TilePressHandler() );

            server.EventHandlers.Subscribe(new SnowPressHandler() );

            server.EventHandlers.Subscribe(new CampfireHandler() );

            server.EventHandlers.Subscribe(new FireFieldHandler() );

            server.EventHandlers.Subscribe(new PoisonFieldHandler() );

            server.EventHandlers.Subscribe(new EnergyFieldHandler() );

            server.EventHandlers.Subscribe(new TrapHandler() );

            server.EventHandlers.Subscribe(new JungleMawHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}