using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new UseItemWithCreatureWalkToSourceHandler() );

            server.CommandHandlers.Add(new FarAwayRunes2Handler() );

            server.CommandHandlers.Add(new UseItemWithCreatureWalkToTargetHandler() );            

            server.CommandHandlers.Add(new SmallHealthPotionHandler() );

            server.CommandHandlers.Add(new HealthPotionHandler() );

            server.CommandHandlers.Add(new GreatHealthPotionHandler() );

            server.CommandHandlers.Add(new StrongHealthPotionHandler() );

            server.CommandHandlers.Add(new UltimateHealthPotionHandler() );

            server.CommandHandlers.Add(new ManaPotionHandler() );

            server.CommandHandlers.Add(new GreatManaPotionHandler() );

            server.CommandHandlers.Add(new StrongManaPotionHandler() );

            server.CommandHandlers.Add(new GreatSpiritPotionHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}