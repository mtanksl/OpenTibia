using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CleanUpWindowCollectionHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    foreach (var pair in player.Client.Windows.GetIndexedWindows() )
                    {
                        player.Client.Windows.CloseWindow(pair.Key);                    
                    }

                    return Promise.Completed;
                } );
            }

            return next();            
        }
    }
}