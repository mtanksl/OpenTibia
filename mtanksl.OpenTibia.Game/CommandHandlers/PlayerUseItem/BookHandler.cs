using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BookHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> books = new HashSet<ushort>() { 1955 };

        public override bool CanHandle(PlayerUseItemCommand command, Server server)
        {
            if (books.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Server server)
        {
            return new ConditionalCommand(context =>
            {
                 context.WritePacket(command.Player.Client.Connection, new OpenTextDialogOutgoingPacket(0, command.Item.Metadata.TibiaId, (ushort)( (ReadableItem)command.Item ).Text.Length, ( (ReadableItem)command.Item ).Text, "", "") );
               
                return true;
            } );
        }
    }
}