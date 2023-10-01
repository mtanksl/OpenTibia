using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class TileAddCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new ProtectionZoneHandler() );

            Context.Server.EventHandlers.Subscribe(new TilePressHandler() );

            Context.Server.EventHandlers.Subscribe(new SnowPressHandler() );

            Context.Server.EventHandlers.Subscribe(new CampfireHandler() );

            Context.Server.EventHandlers.Subscribe(new FireFieldHandler() );

            Context.Server.EventHandlers.Subscribe(new PoisonFieldHandler() );

            Context.Server.EventHandlers.Subscribe(new EnergyFieldHandler() );

            Context.Server.EventHandlers.Subscribe(new OpenTrapHandler() );

            Context.Server.EventHandlers.Subscribe(new JungleMawHandler() );

            Context.Server.EventHandlers.Subscribe(new BladesHandler() );

            Context.Server.EventHandlers.Subscribe(new SpikesHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}