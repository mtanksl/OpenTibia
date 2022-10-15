using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new UseItemWalkToSourceHandler() );

            server.CommandHandlers.Add(new ContainerOpenHandler() );

            server.CommandHandlers.Add(new FoodHandler() );

            server.CommandHandlers.Add(new LadderHandler() );

            server.CommandHandlers.Add(new SewerHandler() );

            server.CommandHandlers.Add(new BlueberryBushHandler() );

            server.CommandHandlers.Add(new BookHandler() );

            server.CommandHandlers.Add(new WatchHandler() );

            server.CommandHandlers.Add(new UseItemTransformHandler() );

            server.CommandHandlers.Add(new LockedDoorHandler() );

            server.CommandHandlers.Add(new OpenDoorHandler() );

            server.CommandHandlers.Add(new CloseDoorHandler() );

            server.CommandHandlers.Add(new GoldCoinHandler() );

            server.CommandHandlers.Add(new PlatinumCoinHandler() );

            server.CommandHandlers.Add(new DiceHandler() );

            server.CommandHandlers.Add(new SurpriseBagBlueHandler() );

            server.CommandHandlers.Add(new SurpriseBagRedHandler() );

            server.CommandHandlers.Add(new PartyTrumpetHandler() );

            server.CommandHandlers.Add(new PartyHatHandler() );

            server.CommandHandlers.Add(new FireworksRocketHandler() ); 
            
            server.CommandHandlers.Add(new PiggyBankHandler() );    
            
            server.CommandHandlers.Add(new PandaTeddyHandler() );

            server.CommandHandlers.Add(new StuffedDragonHandler() );            
        }

        public void Stop(Server server)
        {
            
        }
    }
}