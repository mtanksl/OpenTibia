using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler(new UseItemWalkToSourceHandler() );

            //TODO: Re-validate rules for incoming packet

            Context.Server.CommandHandlers.AddCommandHandler(new LockerOpenHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ContainerOpenHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BlueberryBushHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new BookHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new ConstructionKitHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CloseDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new CrystalCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new DiceHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FireworksRocketHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new FoodHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GateOfExpertiseHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new GoldCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LadderHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new LockedDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new MusicalInstrumentHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new OpenDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PandaTeddyHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PartyHatHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PartyTrumpetHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PiggyBankHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new PlatinumCoinHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SantaDollHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SealedDoorHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SewerHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SnowHeapHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new StuffedDragonHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SurpriseBagBlueHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new SurpriseBagRedHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new UseItemTransformHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new WatchHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new WindowHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}