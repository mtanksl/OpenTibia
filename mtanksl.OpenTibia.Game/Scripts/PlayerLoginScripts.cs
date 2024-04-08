using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerLoginScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new WelcomeHandler() );

            Context.Server.EventHandlers.Subscribe<PlayerLoginEventArgs>( (context, e) =>
            {
                Context.Server.Logger.WriteLine(e.Player.Name + " login.", LogLevel.Information);

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            
        }
    }
}