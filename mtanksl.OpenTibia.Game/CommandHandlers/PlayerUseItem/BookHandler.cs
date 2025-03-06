using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class BookHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is ReadableItem readableItem && (command.Item.Metadata.Flags.Is(ItemMetadataFlags.Writeable) || command.Item.Metadata.Flags.Is(ItemMetadataFlags.Readable) ) )
            {
                foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                {
                    command.Player.Client.Windows.CloseWindow(pair.Key);
                }

                Window window = new Window();

                window.Item = readableItem;

                uint windowId = command.Player.Client.Windows.OpenWindow(window);

                Context.AddPacket(command.Player, new OpenShowOrEditTextDialogOutgoingPacket(windowId, command.Item.Metadata.TibiaId, Constants.MaxBookCharacters, readableItem.Text, readableItem.WrittenBy, DateTimeOffset.FromUnixTimeSeconds(readableItem.WrittenDate).ToString("dd/MM/yyyy HH:mm:ss") ) );

                return Promise.Completed;
            }

            return next();
        }
    }
}