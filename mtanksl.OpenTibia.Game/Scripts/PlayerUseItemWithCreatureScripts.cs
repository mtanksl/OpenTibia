using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class PlayerUseItemWithCreatureScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new HealthPotionHandler() );

            server.CommandHandlers.Add(new ManaPotionHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}