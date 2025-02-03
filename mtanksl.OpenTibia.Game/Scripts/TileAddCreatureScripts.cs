using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class TileAddCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<TileAddCreatureCommand>(new TileAddingCreatureScriptingHandler() );


            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new TileAddCreatureScriptingHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new ProtectionZoneHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new NoLogoutZoneHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new OceanFloorHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new TilePressHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new SnowPressHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new SearingFireHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new CampfireHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new FireFieldHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new PoisonFieldHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new EnergyFieldHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new OpenTrapHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new JungleMawHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new BladesHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new SpikesHandler() );

            Context.Server.EventHandlers.Subscribe<TileAddCreatureEventArgs>(new SwimEnterHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}