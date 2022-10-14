using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerSayScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new CreateMonsterHandler() );

            server.CommandHandlers.Add(new CreateNpcHandler() );

            server.CommandHandlers.Add(new DestroyMonsterOrNpcHandler() );

            server.CommandHandlers.Add(new CreateItemHandler() );

            server.CommandHandlers.Add(new TeleportDownHandler() );

            server.CommandHandlers.Add(new TeleportHandler() );

            server.CommandHandlers.Add(new TeleportUpHandler() );

            server.CommandHandlers.Add(new TeleportToTownHandler() );
            
            server.CommandHandlers.Add(new TeleportToWaypointHandler() );

            server.CommandHandlers.Add(new SpellHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}