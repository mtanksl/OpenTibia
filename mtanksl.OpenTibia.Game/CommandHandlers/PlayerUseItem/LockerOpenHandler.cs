using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class LockerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is Locker mapLocker)
            {
                Locker locker = Context.Server.Lockers.GetLocker(command.Player.DatabasePlayerId, mapLocker.TownId);

                if (locker == null)
                {
                    locker = (Locker)Context.Server.ItemFactory.Create(2591, 1);

                    locker.TownId = mapLocker.TownId;

                    Context.Server.ItemFactory.Attach(locker);

                    Item depot = Context.Server.ItemFactory.Create(2594, 1);

                    Context.Server.ItemFactory.Attach(depot);

                    locker.AddContent(depot);

                    Context.Server.Lockers.AddLocker(command.Player.DatabasePlayerId, locker);
                }

                command.Item = locker;
            }

            return next();
        }
    }
}