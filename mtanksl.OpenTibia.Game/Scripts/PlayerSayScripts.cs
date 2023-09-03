using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerSayScripts : Script
    {
        public override void Start()
        {
            foreach (var plugin in Context.Server.Plugins.PlayerSayPlugins.Values)
            {
                plugin.Start();
            }

            Context.Server.CommandHandlers.AddCommandHandler(new PlayerSayScriptingHandler(Context.Server.Plugins.PlayerSayPlugins) );

            Context.Server.CommandHandlers.AddCommandHandler(new SpellsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new DisplayMagicEffectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new DisplayProjectileTypeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new KickPlayerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportDownHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportToPlayerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportUpHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CreateItemHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CreateMonsterHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CreateNpcHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new DestroyMonsterNpcItemHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new InvisibleHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportPlayerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportToTownHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new TeleportToWaypointHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BanPlayerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UnBanPlayerHandler() );
        }

        public override void Stop()
        {
            foreach (var plugin in Context.Server.Plugins.PlayerSayPlugins.Values)
            {
                plugin.Stop();
            }
        }
    }
}