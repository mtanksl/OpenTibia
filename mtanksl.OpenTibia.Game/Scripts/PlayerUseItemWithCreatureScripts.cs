using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new UseItemWithCreatureWalkToSourceHandler() );

            server.CommandHandlers.Add(new HealthPotionHandler() );

            server.CommandHandlers.Add(new ManaPotionHandler() );

            server.CommandHandlers.Add(new UseItemWithCreatureWalkToTargetHandler() );

            
        }

        public void Stop(Server server)
        {
            
        }
    }
}