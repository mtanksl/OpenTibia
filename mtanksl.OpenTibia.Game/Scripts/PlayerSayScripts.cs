using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerSayScripts : Script
    {
        public override void Start(Server server)
        {
            server.CommandHandlers.Add(new SpellsHandler() );

            server.CommandHandlers.Add(new DisplayMagicEffectHandler() );

             server.CommandHandlers.Add(new DisplayProjectileTypeHandler() );

            server.CommandHandlers.Add(new KickPlayerHandler() );

            server.CommandHandlers.Add(new TeleportDownHandler() );

            server.CommandHandlers.Add(new TeleportToPlayerHandler() );

            server.CommandHandlers.Add(new TeleportUpHandler() );

            server.CommandHandlers.Add(new CreateItemHandler() );

            server.CommandHandlers.Add(new CreateMonsterHandler() );

            server.CommandHandlers.Add(new CreateNpcHandler() );

            server.CommandHandlers.Add(new DestroyMonsterOrNpcHandler() );

            server.CommandHandlers.Add(new InvisibleHandler() );

            server.CommandHandlers.Add(new TeleportHandler() );

            server.CommandHandlers.Add(new TeleportPlayerHandler() );

            server.CommandHandlers.Add(new TeleportToTownHandler() );

            server.CommandHandlers.Add(new TeleportToWaypointHandler() );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}