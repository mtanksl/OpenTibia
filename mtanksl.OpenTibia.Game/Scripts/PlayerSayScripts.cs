using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class PlayerSayScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new PlayerSayScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new AccountManagerSayHandler() );

            var gamemasterCommandHandler = new GamemasterCommandHandler();

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportHandler() ); //a

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new BanPlayerHandler() ); //ban

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportPlayerHandler() ); //c

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportDownHandler() ); //down

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new InvisibleHandler() ); //ghost

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToPlayerHandler() ); //goto

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new CreateItemHandler() ); //i

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new KickPlayerHandler() ); //kick

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new DisplayMagicEffectHandler() ); //me

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new CreateMonsterHandler() ); //m

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new CreateNpcHandler() ); //n

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new DisplayProjectileTypeHandler() ); //pe

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new DestroyMonsterNpcItemHandler() ); //r

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToTownHandler() ); //town

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToHomeTownHandler() ); //t

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new UnBanPlayerHandler() ); //unban

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportUpHandler() ); //up

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToWaypointHandler() ); //w
            
            Context.Server.CommandHandlers.AddCommandHandler(gamemasterCommandHandler);

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new SpellsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new EditHouseSubOwnerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new EditHouseGuestHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new EditHouseDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new HouseKickHandler() );
        }

        public override void Stop()
        {

        }
    }
}