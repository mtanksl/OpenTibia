using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithItemWalkToSourceHandler() );

            //TODO: Re-validate rules for incoming packet

            //TODO: "You cannot use there."

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithItemScriptingHandler(true) );

            Context.Server.CommandHandlers.AddCommandHandler(new Runes2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FishingRodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithItemWalkToTargetHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWithItemScriptingHandler(false) );

            Context.Server.CommandHandlers.AddCommandHandler(new BakingTrayWithDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BunchOfSugarCaneHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new DestroyFieldHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FireBugHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FlourHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FluidItem2Handler() );

            Context.Server.CommandHandlers.AddCommandHandler(new JuiceSqueezerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new KeyHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new KnifeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfCakeDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfChocolateDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LumpOfDoughHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MacheteHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PickHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new RopeHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ScytheHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ShovelHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SickleHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new WheatHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}