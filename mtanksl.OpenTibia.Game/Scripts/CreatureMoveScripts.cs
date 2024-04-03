using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureMoveScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new CreatureMoveContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CreatureMoveTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new HouseTileHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MagicForcefieldHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new HoleHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PitfallHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StairsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}