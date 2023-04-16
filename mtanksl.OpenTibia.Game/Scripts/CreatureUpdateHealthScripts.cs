using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class CreatureUpdateHealthScripts : Script
    {
        public override void Start(Server server)
        {
            server.CommandHandlers.Add(new DeathHandler() );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}