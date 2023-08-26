using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyContainerCloseHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                foreach (var pair in command.Player.Client.Containers.GetIndexedContainers() )
                {
                    command.Player.Client.Containers.CloseContainer(pair.Key);
                }

                return Promise.Completed;
            } );
        }
    }
}