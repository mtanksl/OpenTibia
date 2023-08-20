using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new UseItemWithItemWalkToSourceHandler() );

            //TODO: You cannot use there.

            Context.Server.CommandHandlers.Add(new Runes2Handler() );

            Context.Server.CommandHandlers.Add(new FishingRodHandler() );

            Context.Server.CommandHandlers.Add(new UseItemWithItemWalkToTargetHandler() );

            Context.Server.CommandHandlers.Add(new BakingTrayWithDoughHandler() );

            Context.Server.CommandHandlers.Add(new BunchOfSugarCaneHandler() );

            Context.Server.CommandHandlers.Add(new DestroyFieldHandler() );

            Context.Server.CommandHandlers.Add(new FireBugHandler() );

            Context.Server.CommandHandlers.Add(new FlourHandler() );

            Context.Server.CommandHandlers.Add(new FluidItem2Handler() );

            Context.Server.CommandHandlers.Add(new JuiceSqueezerHandler() );

            Context.Server.CommandHandlers.Add(new KeyHandler() );

            Context.Server.CommandHandlers.Add(new KnifeHandler() );

            Context.Server.CommandHandlers.Add(new LumpOfCakeDoughHandler() );

            Context.Server.CommandHandlers.Add(new LumpOfChocolateDoughHandler() );

            Context.Server.CommandHandlers.Add(new LumpOfDoughHandler() );

            Context.Server.CommandHandlers.Add(new MacheteHandler() );

            Context.Server.CommandHandlers.Add(new PickHandler() );

            Context.Server.CommandHandlers.Add(new RopeHandler() );

            Context.Server.CommandHandlers.Add(new ScytheHandler() );

            Context.Server.CommandHandlers.Add(new ShovelHandler() );

            Context.Server.CommandHandlers.Add(new SickleHandler() );

            Context.Server.CommandHandlers.Add(new WheatHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}