using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerSayScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new SpellsHandler() );

            Context.Server.CommandHandlers.Add(new DisplayMagicEffectHandler() );

            Context.Server.CommandHandlers.Add(new DisplayProjectileTypeHandler() );

            Context.Server.CommandHandlers.Add(new KickPlayerHandler() );

            Context.Server.CommandHandlers.Add(new TeleportDownHandler() );

            Context.Server.CommandHandlers.Add(new TeleportToPlayerHandler() );

            Context.Server.CommandHandlers.Add(new TeleportUpHandler() );

            Context.Server.CommandHandlers.Add(new CreateItemHandler() );

            Context.Server.CommandHandlers.Add(new CreateMonsterHandler() );

            Context.Server.CommandHandlers.Add(new CreateNpcHandler() );

            Context.Server.CommandHandlers.Add(new DestroyMonsterNpcItemHandler() );

            Context.Server.CommandHandlers.Add(new InvisibleHandler() );

            Context.Server.CommandHandlers.Add(new TeleportHandler() );

            Context.Server.CommandHandlers.Add(new TeleportPlayerHandler() );

            Context.Server.CommandHandlers.Add(new TeleportToTownHandler() );

            Context.Server.CommandHandlers.Add(new TeleportToWaypointHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}