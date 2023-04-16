using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLoginScripts : Script
    {
        public override void Start(Server server)
        {
            server.EventHandlers.Subscribe<PlayerLoginEventArgs>( (context, e) =>
            {
                server.Logger.WriteLine(e.Player.Name + " login.", LogLevel.Information);

                return Promise.Completed;
            } );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}