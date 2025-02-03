using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class CreatureMoveScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new CreatureMoveContainerCloseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new CreatureMoveTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new CreatureMoveTileRemovingScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new CreatureMoveTileAddingScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new HouseTileHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new ProtectionZoneBlockHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new MagicForcefieldHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new HoleHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new PitfallHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureMoveCommand>(new StairsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}