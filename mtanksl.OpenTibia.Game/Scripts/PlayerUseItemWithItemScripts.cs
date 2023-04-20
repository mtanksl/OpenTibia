using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithItemScripts : Script
    {
        public override void Start(Server server)
        {
            server.CommandHandlers.Add(new UseItemWithItemWalkToSourceHandler() );

            //TODO: You cannot use there.

            server.CommandHandlers.Add(new Runes2Handler() );

            server.CommandHandlers.Add(new FishingRodHandler() );

            server.CommandHandlers.Add(new UseItemWithItemWalkToTargetHandler() );

            server.CommandHandlers.Add(new BakingTrayWithDoughHandler() );

            server.CommandHandlers.Add(new FluidItemHandler() );

            server.CommandHandlers.Add(new BunchOfSugarCaneHandler() );

            server.CommandHandlers.Add(new DestroyFieldHandler() );

            server.CommandHandlers.Add(new FireBugHandler() );

            server.CommandHandlers.Add(new FlourHandler() );

            server.CommandHandlers.Add(new KnifeHandler() );

            server.CommandHandlers.Add(new LumpOfCakeDoughHandler() );

            server.CommandHandlers.Add(new LumpOfChocolateDoughHandler() );

            server.CommandHandlers.Add(new LumpOfDoughHandler() );

            server.CommandHandlers.Add(new MacheteHandler() );

            server.CommandHandlers.Add(new PickHandler() );

            server.CommandHandlers.Add(new RopeHandler() );

            server.CommandHandlers.Add(new ScytheHandler() );

            server.CommandHandlers.Add(new ShovelHandler() );

            server.CommandHandlers.Add(new SickleHandler() );

            server.CommandHandlers.Add(new WheatHandler() );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}