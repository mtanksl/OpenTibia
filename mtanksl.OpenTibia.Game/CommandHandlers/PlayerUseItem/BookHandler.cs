using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BookHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> books = new HashSet<ushort>() { 2599, 1955, 2597 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (books.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                ReadableItem readableItem = (ReadableItem)command.Item;

                Window window = new Window();

                window.AddContent(readableItem);

                uint windowId = command.Player.Client.Windows.OpenWindow(window);

                Context.AddPacket(command.Player, new OpenShowOrEditTextDialogOutgoingPacket(windowId, command.Item.Metadata.TibiaId, 1024, readableItem.Text, readableItem.Author, readableItem.Date) );

                return Promise.Completed;
            }

            return next();
        }
    }
}