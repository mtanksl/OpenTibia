using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public class CreatureDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new PlayerDestroyTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new NpcDestroyNpcTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new CleanUpChannelCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new CleanUpPartyCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new CleanUpRuleViolationCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new CleanUpContainerCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new CleanUpWindowCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<CreatureDestroyCommand>(new DeathHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}