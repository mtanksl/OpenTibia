using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureMoveScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new MoveAwayContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MoveAwayTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MoveAwayNpcTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MagicForcefieldHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new HoleHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PitfallHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StairsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SwimHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}