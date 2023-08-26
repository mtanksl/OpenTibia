using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyWindowCloseHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                {
                    command.Player.Client.Windows.CloseWindow(pair.Key);                    
                }

                return Promise.Completed;
            } );
        }
    }
}