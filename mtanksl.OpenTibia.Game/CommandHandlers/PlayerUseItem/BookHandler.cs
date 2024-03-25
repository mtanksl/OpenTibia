using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class BookHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is ReadableItem readableItem)
            {
                foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                {
                    command.Player.Client.Windows.CloseWindow(pair.Key);
                }

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