using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerDeathScripts : Script
    {
        public override void Start(Server server)
        {
            server.EventHandlers.Subscribe<PlayerDeathEventArgs>( (context, e) =>
            {
                server.Logger.WriteLine(e.Player.Name + " logout.", LogLevel.Information);

                return Promise.Completed;
            } );
        }

        public override void Stop(Server server)
        {
            
        }
    }
}