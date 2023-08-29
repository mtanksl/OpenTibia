using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class LockerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is Locker locker)
            {
                Container container = Context.Server.Lockers.GetLocker(command.Player.DatabasePlayerId, locker.TownId);

                if (container == null)
                {
                    container = (Container)Context.Server.ItemFactory.Create(2591, 1);

                    container.AddContent( Context.Server.ItemFactory.Create(2594, 1) );

                    Context.Server.Lockers.AddLocker(command.Player.DatabasePlayerId, locker.TownId, container);
                }

                command.Item = container;
            }

            return next();
        }
    }
}