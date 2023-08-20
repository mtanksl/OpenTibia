using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.Add(new UseItemWalkToSourceHandler() );

            Context.Server.CommandHandlers.Add(new ContainerOpenHandler() );

            Context.Server.CommandHandlers.Add(new BlueberryBushHandler() );

            Context.Server.CommandHandlers.Add(new BookHandler() );

            Context.Server.CommandHandlers.Add(new ConstructionKitHandler() );

            Context.Server.CommandHandlers.Add(new CloseDoorHandler() );

            Context.Server.CommandHandlers.Add(new CrystalCoinHandler() );

            Context.Server.CommandHandlers.Add(new DiceHandler() );

            Context.Server.CommandHandlers.Add(new FireworksRocketHandler() );

            Context.Server.CommandHandlers.Add(new FoodHandler() );

            Context.Server.CommandHandlers.Add(new GateOfExpertiseHandler() );

            Context.Server.CommandHandlers.Add(new GoldCoinHandler() );

            Context.Server.CommandHandlers.Add(new LadderHandler() );

            Context.Server.CommandHandlers.Add(new LockedDoorHandler() );

            Context.Server.CommandHandlers.Add(new MusicalInstrumentHandler() );

            Context.Server.CommandHandlers.Add(new OpenDoorHandler() );

            Context.Server.CommandHandlers.Add(new PandaTeddyHandler() );

            Context.Server.CommandHandlers.Add(new PartyHatHandler() );

            Context.Server.CommandHandlers.Add(new PartyTrumpetHandler() );

            Context.Server.CommandHandlers.Add(new PiggyBankHandler() );

            Context.Server.CommandHandlers.Add(new PlatinumCoinHandler() );

            Context.Server.CommandHandlers.Add(new SantaDollHandler() );

            Context.Server.CommandHandlers.Add(new SealedDoorHandler() );

            Context.Server.CommandHandlers.Add(new SewerHandler() );

            Context.Server.CommandHandlers.Add(new SnowHeapHandler() );

            Context.Server.CommandHandlers.Add(new StuffedDragonHandler() );

            Context.Server.CommandHandlers.Add(new SurpriseBagBlueHandler() );

            Context.Server.CommandHandlers.Add(new SurpriseBagRedHandler() );

            Context.Server.CommandHandlers.Add(new UseItemTransformHandler() );

            Context.Server.CommandHandlers.Add(new WatchHandler() );

            Context.Server.CommandHandlers.Add(new WindowHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}