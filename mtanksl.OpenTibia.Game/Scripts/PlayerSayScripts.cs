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
                                       
                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new DisplayAnimatedTextHandler() ); // /at

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportHandler() ); // /a

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new BanPlayerHandler() ); // /ban

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportPlayerHandler() ); // /c

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportDownHandler() ); // /down              (alani hur down)

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new InvisibleHandler() ); // /ghost

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToPlayerHandler() ); // /goto          (alani sio)

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new CreateItemHandler() ); // /i

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new KickPlayerHandler() ); // /kick                (omana)

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new DisplayMagicEffectHandler() ); // /me

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new CreateMonsterHandler() ); // /m

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new CreateNpcHandler() ); // /n

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new DisplayProjectileTypeHandler() ); // /pe

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new DestroyMonsterNpcItemHandler() ); // /r        (alito tera)

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToTownHandler() ); // /town

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToHomeTownHandler() ); // /t           (omani)

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new UnBanPlayerHandler() ); // /unban

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportUpHandler() ); // /up                  (alani hur up)

                gamemasterCommandHandler.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new TeleportToWaypointHandler() ); // /w           (alani)

            Context.Server.CommandHandlers.AddCommandHandler(gamemasterCommandHandler);

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new HelpHandler() ); // !help

            if (Context.Server.Config.Rules != null)
            {
                Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new RulesHandler() ); // !rules
            }

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new OnlineHandler() ); // !online

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new ServerInfoHandler() ); // !serverinfo

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new UptimeHandler() ); // !uptime

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new SpellsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new EditHouseSubOwnerHandler() ); // aleta som

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new EditHouseGuestHandler() ); // aleta sio

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new EditHouseDoorHandler() ); // aleta grav

            Context.Server.CommandHandlers.AddCommandHandler<PlayerSayCommand>(new HouseKickHandler() ); // alana sio
        }

        public override void Stop()
        {

        }
    }
}