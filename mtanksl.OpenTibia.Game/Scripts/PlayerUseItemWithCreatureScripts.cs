using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithCreatureWalkToSourceHandler() );

            //TODO: Re-validate rules for incoming packet

            //TODO: "You cannot use there."

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithCreatureScriptingHandler(true) );

            Context.Server.CommandHandlers.AddCommandHandler(new RunesHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithCreatureWalkToTargetHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithCreatureScriptingHandler(false) );

            Context.Server.CommandHandlers.AddCommandHandler(new SmallHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new HealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GreatHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StrongHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UltimateHealthPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GreatManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StrongManaPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GreatSpiritPotionHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FluidItemHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}