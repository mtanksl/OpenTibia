using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BookHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> books = new HashSet<ushort>() { 1955 };

        public override bool CanHandle(Context context, PlayerUseItemCommand command)
        {
            if (books.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemCommand command)
        {
            context.AddPacket(command.Player.Client.Connection, new OpenTextDialogOutgoingPacket(0, command.Item.Metadata.TibiaId, 255, ( (ReadableItem)command.Item ).Text, "", "") );

            base.Handle(context, command);
        }
    }
}