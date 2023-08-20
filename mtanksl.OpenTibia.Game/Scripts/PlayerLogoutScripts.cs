using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLogoutScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<PlayerLogoutEventArgs>( (context, e) =>
            {
                Context.Server.Logger.WriteLine(e.Player.Name + " logout.", LogLevel.Information);

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            
        }
    }
}