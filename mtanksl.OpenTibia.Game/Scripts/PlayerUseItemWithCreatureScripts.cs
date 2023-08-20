using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithCreatureScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new UseItemWithCreatureWalkToSourceHandler() );

            //TODO: You cannot use there.

            Context.Server.CommandHandlers.Add(new RunesHandler() );

            Context.Server.CommandHandlers.Add(new UseItemWithCreatureWalkToTargetHandler() );

            Context.Server.CommandHandlers.Add(new SmallHealthPotionHandler() );

            Context.Server.CommandHandlers.Add(new HealthPotionHandler() );

            Context.Server.CommandHandlers.Add(new GreatHealthPotionHandler() );

            Context.Server.CommandHandlers.Add(new StrongHealthPotionHandler() );

            Context.Server.CommandHandlers.Add(new UltimateHealthPotionHandler() );

            Context.Server.CommandHandlers.Add(new ManaPotionHandler() );

            Context.Server.CommandHandlers.Add(new GreatManaPotionHandler() );

            Context.Server.CommandHandlers.Add(new StrongManaPotionHandler() );

            Context.Server.CommandHandlers.Add(new GreatSpiritPotionHandler() );

            Context.Server.CommandHandlers.Add(new FluidItemHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}