using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerSayScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new PlayerSayScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SpellsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportHandler() ); //a

            Context.Server.CommandHandlers.AddCommandHandler(new BanPlayerHandler() ); //ban

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportPlayerHandler() ); //c

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportDownHandler() ); //down

            Context.Server.CommandHandlers.AddCommandHandler(new InvisibleHandler() ); //ghost

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportToPlayerHandler() ); //goto

            Context.Server.CommandHandlers.AddCommandHandler(new CreateItemHandler() ); //i

            Context.Server.CommandHandlers.AddCommandHandler(new KickPlayerHandler() ); //kick

            Context.Server.CommandHandlers.AddCommandHandler(new DisplayMagicEffectHandler() ); //me

            Context.Server.CommandHandlers.AddCommandHandler(new CreateMonsterHandler() ); //m

            Context.Server.CommandHandlers.AddCommandHandler(new CreateNpcHandler() ); //n

            Context.Server.CommandHandlers.AddCommandHandler(new DisplayProjectileTypeHandler() ); //pe

            Context.Server.CommandHandlers.AddCommandHandler(new DestroyMonsterNpcItemHandler() ); //r

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportToTownHandler() ); //t

            Context.Server.CommandHandlers.AddCommandHandler(new UnBanPlayerHandler() ); //unban

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportUpHandler() ); //up

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportToWaypointHandler() ); //w
        }

        public override void Stop()
        {

        }
    }
}