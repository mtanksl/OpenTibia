using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class SafeHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if ( (command.ToContainer is Container toContainer && toContainer.Root() is Safe) && !(command.Item.Root() is Safe) )
            {
                foreach (var pair in command.Player.Lockers.GetIndexedContents() )
                {
                    Locker locker = (Locker)pair.Value;

                    if (toContainer.IsContentOf(locker) )
                    {
                        int maxDepotItems = command.Player.Premium ? Context.Server.Config.GameplayDepotPremiumLimit : Context.Server.Config.GameplayDepotFreeLimit;

                        if (command.Item is Container container)
                        {
                            if (Sum(locker) + Sum(container) >= maxDepotItems)
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YourDepotIsFull) );

                                return Promise.Break;
                            }
                        }
                        else
                        {
                            if (Sum(locker) >= maxDepotItems)
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YourDepotIsFull) );

                                return Promise.Break;
                            }
                        }

                        break;
                    }
                }
            }

            return next();
        }

        private static int Sum(IContainer parent)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                sum += 1;

                if (content is Container container)
                {
                    sum += Sum(container);
                }
            }

            return sum;
        }
    }
}