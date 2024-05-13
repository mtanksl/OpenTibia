using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class LockerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is Locker mapLocker)
            {
                Locker locker = (Locker)command.Player.Lockers.GetContent(mapLocker.TownId);

                if (locker == null)
                {
                    locker = (Locker)Context.Server.ItemFactory.Create(Constants.LockerOpenTibiaItemId, 1);

                    locker.TownId = mapLocker.TownId;

                    Context.Server.ItemFactory.Attach(locker);

                    Item depot = Context.Server.ItemFactory.Create(Constants.DepotOpenTibiaItemId, 1);

                    Context.Server.ItemFactory.Attach(depot);

                    locker.AddContent(depot);

                    command.Player.Lockers.AddContent(locker, locker.TownId);
                }

                command.Item = locker;
            }

            return next();
        }
    }
}