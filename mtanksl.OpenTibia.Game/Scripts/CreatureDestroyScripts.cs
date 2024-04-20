using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureDestroyScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new PlayerDestroyTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new NpcDestroyNpcTradingRejectHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CleanUpChannelCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CleanUpPartyCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CleanUpRuleViolationCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CleanUpContainerCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CleanUpWindowCollectionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LootHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ExperienceHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}