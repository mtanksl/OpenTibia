using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class TalkScripts : IScript
    {
        public void Start(Server server)
        {
            server.CommandHandlers.Add(new CreateCreatureHandler() );

            server.CommandHandlers.Add(new CreateItemHandler() );

            server.CommandHandlers.Add(new TeleportDownHandler() );

            server.CommandHandlers.Add(new TeleportHandler() );

            server.CommandHandlers.Add(new TeleportUpHandler() );
        }

        public void Stop(Server server)
        {
            
        }
    }
}