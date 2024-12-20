using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerSayScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new PlayerSayScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new AccountManagerSayHandler() );

            var gamemasterCommandHandler = new GamemasterCommandHandler();

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new TeleportHandler() ); //a

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new BanPlayerHandler() ); //ban

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new TeleportPlayerHandler() ); //c

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new TeleportDownHandler() ); //down

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new InvisibleHandler() ); //ghost

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new TeleportToPlayerHandler() ); //goto

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new CreateItemHandler() ); //i

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new KickPlayerHandler() ); //kick

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new DisplayMagicEffectHandler() ); //me

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new CreateMonsterHandler() ); //m

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new CreateNpcHandler() ); //n

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new DisplayProjectileTypeHandler() ); //pe

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new DestroyMonsterNpcItemHandler() ); //r

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new TeleportToTownHandler() ); //t

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new UnBanPlayerHandler() ); //unban

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new TeleportUpHandler() ); //up

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler(new TeleportToWaypointHandler() ); //w
            
            Context.Server.CommandHandlers.AddCommandHandler(gamemasterCommandHandler);

            Context.Server.CommandHandlers.AddCommandHandler(new SpellsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new EditHouseSubOwnerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new EditHouseGuestHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new EditHouseDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new HouseKickHandler() );
        }

        public override void Stop()
        {

        }
    }
}